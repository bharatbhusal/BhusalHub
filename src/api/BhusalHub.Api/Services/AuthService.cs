using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BhusalHub.Core.Interfaces;
using BhusalHub.Core.Models;
using BhusalHub.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BhusalHub.Api.Services;

public class AuthService : IAuthService
{
    private readonly DatabaseInitializer _db;
    private readonly IConfiguration _configuration;

    public AuthService(DatabaseInitializer db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        using var conn = _db.CreateConnection();
        var existing = await conn.QueryFirstOrDefaultAsync<string>(
            "SELECT Id FROM User WHERE Username = @Username", new { request.Username });

        if (existing is not null)
            throw new InvalidOperationException("Username already taken.");

        var user = new User
        {
            Id = Guid.NewGuid().ToString("N"),
            Username = request.Username,
            PasswordHash = HashPassword(request.Password),
            DisplayName = string.IsNullOrWhiteSpace(request.DisplayName) ? request.Username : request.DisplayName,
            Role = "user",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        await conn.ExecuteAsync("""
            INSERT INTO User (Id, Username, PasswordHash, DisplayName, Role, CreatedAt, UpdatedAt)
            VALUES (@Id, @Username, @PasswordHash, @DisplayName, @Role, @CreatedAt, @UpdatedAt)
            """, user);

        return MapToResponse(user);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        using var conn = _db.CreateConnection();
        var user = await conn.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM User WHERE Username = @Username", new { request.Username });

        if (user is null || !VerifyPassword(request.Password, user.PasswordHash))
            return null;

        return MapToResponse(user);
    }

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM User WHERE Id = @Id", new { Id = userId });
    }

    public string GenerateToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var expireDays = int.TryParse(jwtSettings["ExpireDays"], out var days) ? days : 30;

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(expireDays),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private AuthResponse MapToResponse(User user) => new()
    {
        Token = GenerateToken(user),
        Id = user.Id,
        Username = user.Username,
        DisplayName = user.DisplayName,
        Role = user.Role,
    };

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password, salt, 100_000, HashAlgorithmName.SHA256, 32);
        return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hash);
    }

    private static bool VerifyPassword(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split('.');
        if (parts.Length != 2) return false;
        var salt = Convert.FromBase64String(parts[0]);
        var storedHash = Convert.FromBase64String(parts[1]);
        var computedHash = Rfc2898DeriveBytes.Pbkdf2(
            password, salt, 100_000, HashAlgorithmName.SHA256, 32);
        return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
    }
}
