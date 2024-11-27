namespace MyApi.Application.Handlers.Products.Commands.CreateProductWithImages
{
    public class CreateProductWithImagesResponse
    {
        public int ProductId { get; set; }
        public string? IxoneID { get; set; }
        public string? Upc12 { get; set; }
        public ICollection<CreateProductWithImagesResponseProductImageDto>? ProductImages { get; set; }
    }

    public class CreateProductWithImagesResponseProductImageDto
    {
        public string? OriginalFileName { get; set; }
        public string? ImageUrl { get; set; }
    }
}