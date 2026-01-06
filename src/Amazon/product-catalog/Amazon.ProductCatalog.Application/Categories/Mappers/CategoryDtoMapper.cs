using Amazon.ProductCatalog.Application.Categories.Dtos;
using Amazon.ProductCatalog.Domain.Categories;

namespace Amazon.ProductCatalog.Application.Categories.Mappers;

public static class CategoryDtoMapper
{
    public static CategoryDto ToDto(this Category category)
    {
        var dto = new CategoryDto(category.Id, category.Name, category.ParentCategory?.ToDto() ?? null, category.Children.Select(x => x.Name).ToList());
        return dto;
    }
}