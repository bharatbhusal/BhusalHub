using BhusalHub.Core.Models;

namespace BhusalHub.Core.Interfaces;

public interface IHealthService
{
    HealthResult Check();
}
