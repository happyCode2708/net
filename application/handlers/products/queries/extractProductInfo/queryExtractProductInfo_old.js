using MediatR;

public class GetProductIngredientsQuery : IRequest<ResponseModel<GetProductIngredientsResponse>>
{
  public int ProductItemId { get; set; }
  public GetProductIngredientsQuery(int productItemId)
  {
    ProductItemId = productItemId;
  }

  public class Handler : IRequestHandler<GetProductIngredientsQuery, ResponseModel<GetProductIngredientsResponse>>
  {
    private readonly IProductService _productService;

    public Handler(IProductService productService)
    {
      _productService = productService;
    }

    public async Task<ResponseModel<GetProductIngredientsResponse>> Handle(GetProductIngredientsQuery query, CancellationToken cancellationToken)
    {
      var searchRequest = new SearchProductDetailGridRequest()
      {
        Filter = new SearchProductDetailGridFilter
        {
          GridFilters = new List<GridFilterRequest>
                        {
                            new GridFilterRequest
                            {
                                FieldName = nameof(ProductItemElasticModel.Id).ToCamelCase(),
                                FilterType = GridFilterType.Equal,
                                Value = query.ProductItemId.ToString()
                            }
                        }
        },
        Columns = new List<string>
                    {
                        nameof(ProductItemElasticModel.Id).ToCamelCase(),
                        nameof(ProductItemElasticModel.FoodAndBeverageIngredient).ToCamelCase()
                    }
      };
      var searchResponse = await _productService.SearchProductDetailGrid(searchRequest, false, null, cancellationToken);

      var product = searchResponse.Data?.FirstOrDefault();
      if (product == null)
      {
        return ResponseModel<GetProductIngredientsResponse>.Fail("Unable to find Product Item");
      }

      var result = new GetProductIngredientsResponse()
      {
        ProductId = product.Id,
        IngredientGroups = new List<ProductIngredientGroup>()
      };

      var ingredientStatements = product.FoodAndBeverageIngredient?.IngredientStatement ?? new List<IngredientStatement>();
      for (var index = 0; index < ingredientStatements.Count; index++)
      {
        var ingredientStatement = ingredientStatements[index];
        var ingredientGroup = new ProductIngredientGroup();
        ingredientGroup.Index = index;
        ingredientGroup.GroupIds = ingredientStatement.MultiplePanelsGroupIds;
        if (!string.IsNullOrWhiteSpace(ingredientStatement.Value))
        {
          ingredientGroup.Ingredients = ingredientStatement.Value.Split(',').Select(ingredient => ingredient.Trim()).ToList();
        }

        result.IngredientGroups.Add(ingredientGroup);
      }

      return ResponseModel<GetProductIngredientsResponse>.Success(result);
    }
  }
}
  