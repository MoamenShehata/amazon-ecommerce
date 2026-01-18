using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.ProductCatalog.Application.ReadModel.Models;

public class ProductForListModel(Guid id, string Name, string Categories, decimal UnitPrice) : Entity<Guid>(id)
{ }