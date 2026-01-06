namespace Amazon.ProductCatalog.Application.Categories.Dtos
{
    public record CategoryDto(Guid Id, string Name, CategoryDto ParentCategory = null!, params List<string> Children);

    public record CreateCategoryRequest(string Name, Guid? ParentCategoryId = null);
    public record UpdateCategoryRequest(string Name, Guid? NewParentCategoryId = null);
}