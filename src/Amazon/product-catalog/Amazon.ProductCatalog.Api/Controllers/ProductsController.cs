using Amazon.ProductCatalog.Application.Products;
using Amazon.ProductCatalog.Application.Products.Dtos;
using Amazon.ProductCatalog.Read.Services;
using Amazon.SharedKernel.Common;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(ProductsAppService _productsAppService,
    ICatalogReadServices _catalogReadServices) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProductsPage([FromQuery] PageRequest pageRequest)
    {
        return Ok(await _catalogReadServices.GetProductsPageAsync(pageRequest));
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(
        [FromBody] CreateProductDto request)
    {
        var productCreateResult = await _productsAppService.CreateAsync(request);

        if (productCreateResult.IsSuccess)
            return CreatedAtRoute("GetProductById", new { id = productCreateResult.Value.Id }, productCreateResult.Value);

        return RestResult(productCreateResult);

    }

    [HttpGet("{id}", Name = "GetProductById")]
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
