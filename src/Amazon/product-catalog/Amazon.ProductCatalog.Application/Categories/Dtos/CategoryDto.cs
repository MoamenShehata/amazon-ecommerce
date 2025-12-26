namespace Amazon.ProductCatalog.Application.Categories.Dtos
{
    public record CategoryDto(Guid Id, string Name);
    public record CreateCategoryRequest(string Name, Guid? ParentCategoryId = null);
    public record UpdateCategoryRequest(string Name, Guid? NewParentCategoryId = null);
}