namespace BhusalHub.Core.Models;

public class HealthResult
{
    public string Status { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
