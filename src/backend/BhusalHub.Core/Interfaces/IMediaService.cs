using BhusalHub.Core.Models;

namespace BhusalHub.Core.Interfaces;

public interface IMediaService
{
    Task<IEnumerable<Media>> ListAsync(int page, int pageSize);
    Task<Media?> GetByIdAsync(string id);
}
