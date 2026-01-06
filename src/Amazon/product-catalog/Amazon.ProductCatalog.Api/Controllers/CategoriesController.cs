using Amazon.ProductCatalog.Application.Categories;
using Amazon.ProductCatalog.Application.Categories.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(CategoriesAppService _categoriesAppService) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCategory(
        [FromBody] CreateCategoryRequest request)
    {
        var result = await _categoriesAppService.CreateAsync(request);
        if (result.IsSuccess)
            return CreatedAtRoute("GetCategoryId", new { id = result.Value.Id }, result.Value);

        return RestResult(result);
    }

    [HttpGet("{id}", Name = "GetCategoryId")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        return RestResult(await _categoriesAppService.GetByIdAsync(id));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(
        Guid id,
        [FromBody] UpdateCategoryRequest request)
    {
        return RestResult(await _categoriesAppService.UpdateAsync(id, request));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(
        Guid id,
        [FromQuery] Guid? orphanProductsNewCategoryId = null)
    {
        return RestResult(await _categoriesAppService.DeleteAsync(id, orphanProductsNewCategoryId));
    }
}