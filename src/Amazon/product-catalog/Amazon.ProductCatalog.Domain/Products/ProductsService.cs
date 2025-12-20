using Amazon.ProductCatalog.Domain.Categories;
using Amazon.ProductCatalog.Domain.Products.ValueObjects;
using EMP.SharedKernel.Repositories;

namespace Amazon.ProductCatalog.Domain.Products;

public class ProductsService(
    IRepository<Category, Guid> _categoriesRepository,
    IRepository<Product, Guid> _productsRepository
    )
{
    public async Task<Product> CreateAsync(Guid categoryId, string name, ProductPrice price)
    {
        var category = await _categoriesRepository.GetByIdAsync(categoryId) ?? throw new Exception();

        var productBySameName = await _productsRepository.FilterSingleAsync(p => p.Name.ToLower() == name.ToLower());
        if (productBySameName != null) throw new Exception();

        return category.NewProduct(name, price);
    }
}