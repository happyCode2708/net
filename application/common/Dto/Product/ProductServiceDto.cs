using MyApi.Application.Common.Types.Grid;

namespace Application.Common.Dto.Product
{
    public class ProductGridItem : BaseProductGridItem
    {
        public List<ProductImageDto>? ProductImages { get; set; }
    }

    public class GetProductGridItemReturn
    {
        public required List<ProductGridItem> ProductGridData;
        public int? TotalCount;

    }

    public class GetProductGridProps
    {
        public required GetProductGridFiler Filter;

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