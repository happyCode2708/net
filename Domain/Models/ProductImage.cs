namespace MyApi.Domain.Models
{
    public class ProductImage
    {
        public int ProductId { get; set; }
        public required Product Product { get; set; }
        public int ImageId { get; set; }
        public required Image Image { get; set; }
    }
}