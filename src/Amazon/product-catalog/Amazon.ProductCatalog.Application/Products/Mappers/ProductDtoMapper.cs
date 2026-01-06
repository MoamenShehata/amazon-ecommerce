using Amazon.ProductCatalog.Application.Products.Dtos;
using Amazon.ProductCatalog.Domain.Categories;
using Amazon.ProductCatalog.Domain.Products;

namespace Amazon.ProductCatalog.Application.Products.Mappers;

public static class ProductDtoMapper
{
    public static ProductDto ToDto(this (Product, Category) productModel)
    {
        return new ProductDto(
            productModel.Item1.Id,
            productModel.Item1.CategoryId,
            productModel.Item2.Name,
            productModel.Item1.Name,
            productModel.Item1.Price.Amount,
            productModel.Item1.Properties.ToList(),
            productModel.Item1.CreatedOn,
            productModel.Item1.UpdatedOn
        );
    }
}