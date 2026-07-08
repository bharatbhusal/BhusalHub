namespace BhusalHub.Core.Models;

public class Media
{
    public string Id { get; set; } = string.Empty;
    public string FilenameOriginal { get; set; } = string.Empty;
    public string FilenameStored { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Duration { get; set; }
    public string? ThumbnailPath { get; set; }
    public string Checksum { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UploaderId { get; set; } = string.Empty;
}
