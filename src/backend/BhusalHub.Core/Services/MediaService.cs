using BhusalHub.Core.Interfaces;
using BhusalHub.Core.Models;

namespace BhusalHub.Core.Services;

public class MediaService : IMediaService
{
    public Task<IEnumerable<Media>> ListAsync(int page, int pageSize)
    {
        var items = Enumerable.Empty<Media>();
        return Task.FromResult(items);
    }

    public Task<Media?> GetByIdAsync(string id)
    {
        return Task.FromResult<Media?>(null);
    }
}
