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
        return Created($"/api/categories/{result.Value.Id}", result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
    {
        return Ok();
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