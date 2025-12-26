using Amazon.ProductCatalog.Application.Products;
using Amazon.ProductCatalog.Application.Products.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/categories/{categoryId}/[controller]")]
public class ProductsController(ProductsAppService _productsAppService) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct(
        [FromBody] CreateProductDto request)
    {
        var product = await _productsAppService.CreateAsync(request);
        return RestResult(product);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var product = await _productsAppService.GetByIdAsync(id);
        return RestResult(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(
        Guid id,
        [FromBody] UpdateProductDto request)
    {
        var result = await _productsAppService.UpdateAsync(id, request);
        return RestResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(
        Guid id,
        [FromQuery] bool isSoftDelete = true)
    {
        var result = await _productsAppService.DeleteAsync(id, isSoftDelete);
        return RestResult(result);
    }
}
