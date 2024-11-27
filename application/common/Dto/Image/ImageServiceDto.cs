namespace Application.Common.Dto.Image
{
    public class SaveStaticFileReturn
    {
        public required string OriginFullFileName { get; set; }
        public required string StoredFullFileName { get; set; }
        public required string ImageName { get; set; }
        public int? ThumbnailSize { get; set; }
        public required string Extension { get; set; }
    }
}