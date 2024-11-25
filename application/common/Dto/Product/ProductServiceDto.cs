namespace Application.Common.Dto.Product
{
    public class BaseProductGridItem
    {
        public int Id { get; set; }
        public string UniqueId { get; set; } = Guid.NewGuid().ToString();
        public string? IxoneID { get; set; }
        public string? Upc12 { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class ProductGridItem : BaseProductGridItem
    {
        public List<ProductImageDto>? ProductImages { get; set; }
    }

    public class GetProductGridItemReturn
    {
        public List<ProductGridItem> ProductGridData;
        public int TotalCount;

    }

    public class GetProductGridProps
    {
        public GetProductGridFiler Filter;

        public GetProductGridOptions? Options;
    }

    public class GetProductGridFiler
    {
        public string? SearchText;

        public int? PageNumber;

        public int? PageSize;
    }

    public class GetProductGridOptions
    {
        public bool IncludeImages { get; set; } = false;
    }
}