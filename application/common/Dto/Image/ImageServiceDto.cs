namespace Application.Common.Dto.Image
{
    public class SaveStaticFileReturn
    {
        public string OriginFullFileName { get; set; }
        public string StoredFullFileName { get; set; }
        public string ImageName { get; set; }
        public int? ThumbnailSize { get; set; }
        public string Extension { get; set; }
    }
}