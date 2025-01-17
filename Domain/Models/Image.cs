namespace MyApi.Domain.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string UniqueId { get; set; } = Guid.NewGuid().ToString();
        public required string OriginalFileName { get; set; }
        public required string ImageName { get; set; }
        // Metadata
        public long FileSize { get; set; }
        public required string MimeType { get; set; }
        public bool IsRaw { get; set; }
        public required string Extension { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Navigation property
        public ICollection<ProductImage>? ProductImages { get; set; }
    }
}

