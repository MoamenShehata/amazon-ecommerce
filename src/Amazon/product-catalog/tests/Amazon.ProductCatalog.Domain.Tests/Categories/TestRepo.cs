//using System.Linq.Expressions;
//using Amazon.ProductCatalog.Domain.Categories;
//using Moamen.SDKs.Repository;

//namespace Amazon.ProductCatalog.Domain.Tests.Categories
//{
//    public class TestRepo : IRepository<Category, Guid>
//    {
//        private List<Category> _categories = new()
//        {
//            new Category("Furniture",null),
//            new Category("Electronics",null),
//            new Category("Shoes",null),
//            new Category("Clothes", null),
//        };

//        public void Add(Category aggregate)
//        {
//            _categories.Add(aggregate);
//        }

//        public Task<int> CountAsync(Expression<Func<Category, bool>> predicate)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<IEnumerable<Category>> FilterAsync(Expression<Func<Category, bool>> predicate)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<Category> FilterSingleAsync(Expression<Func<Category, bool>> predicate)
//        {
//            return Task.FromResult(_categories.FirstOrDefault(predicate.Compile()));
//        }

//        public Task<Category> FilterSingleAsync<TProperty>(Expression<Func<Category, bool>> predicate, Func<IQueryable<Category>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Category, TProperty>> include)
//        {
//            return Task.FromResult(_categories.FirstOrDefault(predicate.Compile()));
//        }

//        public Task<IEnumerable<Category>> GetAllAsync()
//        {
//            throw new NotImplementedException();
//        }

//        public Task<Category> GetByIdAsync(Guid id)
//        {
//            return Task.FromResult(_categories.FirstOrDefault(x => x.Id == id));
//        }

//        public Task<Category> GetByIdAsync<TProperty>(Guid id, Expression<Func<Category, TProperty>> include)
//        {
//            throw new NotImplementedException();
//        }

//        public void Remove(Category aggregate)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}