using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.ProductCatalog.Read.Models;

public class ProductForListModel : AuditableEntity<Guid>, IEntity<Guid>
{
    public string Name { get; private set; }
    public string Categories { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string? ImagePath { get; set; }

    public ProductForListModel(Guid id, string Name, string Categories, decimal UnitPrice) : base(id)
    {
        this.Name = Name;
        this.Categories = Categories;
        this.UnitPrice = UnitPrice;
    }



    private ProductForListModel() : base(Guid.Empty) { }
}