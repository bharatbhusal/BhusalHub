using Microsoft.Data.Sqlite;

namespace BhusalHub.Data;

public class DatabaseInitializer
{
    private readonly string _connectionString;

    public DatabaseInitializer(string dbPath)
    {
        _connectionString = $"Data Source={dbPath}";
        Initialize();
    }

    private void Initialize()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS Media (
                Id TEXT PRIMARY KEY,
                FilenameOriginal TEXT NOT NULL,
                FilenameStored TEXT NOT NULL,
                FilePath TEXT NOT NULL,
                MediaType TEXT NOT NULL,
                MimeType TEXT NOT NULL,
                FileSize INTEGER NOT NULL,
                Width INTEGER,
                Height INTEGER,
                Duration INTEGER,
                ThumbnailPath TEXT,
                Checksum TEXT NOT NULL,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NOT NULL,
                UploaderId TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS User (
                Id TEXT PRIMARY KEY,
                Username TEXT NOT NULL UNIQUE,
                PasswordHash TEXT NOT NULL,
                DisplayName TEXT NOT NULL,
                Role TEXT NOT NULL DEFAULT 'user',
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Album (
                Id TEXT PRIMARY KEY,
                Name TEXT NOT NULL,
                Description TEXT DEFAULT '',
                CoverMediaId TEXT,
                OwnerId TEXT NOT NULL,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NOT NULL
            );
            """;
        cmd.ExecuteNonQuery();
    }

    public SqliteConnection CreateConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
