using Moamen.SDKs.Repository;
using MongoDB.Bson;

namespace Amazon.Cart.Infrastructure.Data.Models
{
    public class CustomerClaim : IEntity<ObjectId>
    {
        public ObjectId Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}