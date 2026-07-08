using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BhusalHub.Tests;

public class HealthEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HealthEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Health_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Health_ReturnsValidJson()
    {
        var client = _factory.CreateClient();
        var result = await client.GetFromJsonAsync<HealthResponse>("/api/health");

        Assert.NotNull(result);
        Assert.Equal("healthy", result.Status);
        Assert.Equal("connected", result.Database);
    }

    public record HealthResponse(string Status, string Database, DateTime Timestamp);
}
