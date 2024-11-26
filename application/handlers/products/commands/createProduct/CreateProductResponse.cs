namespace MyApi.Application.Handlers.Products.Commands.CreateProduct
{
    public class CreateProductResponse
    {
        public int ProductId { get; set; }
        public string? IxoneID { get; set; }
        public string? Upc12 { get; set; }
    }
}