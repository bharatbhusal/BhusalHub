using BhusalHub.Core.Interfaces;
using BhusalHub.Core.Models;

namespace BhusalHub.Core.Services;

public class HealthService : IHealthService
{

    private readonly bool _databaseConnected;

    public HealthService()
    {
        _databaseConnected = true;
    }

    public HealthResult Check()
    {
        return new HealthResult
        {
            Status = _databaseConnected ? "healthy" : "unhealthy",
            Database = _databaseConnected ? "connected" : "disconnected",
            Timestamp = DateTime.UtcNow
        };
    }
}
