using System.Linq.Expressions;
using System.Threading.Tasks;
using Amazon.ProductCatalog.Domain.Categories;
using EMP.SharedKernel.Repositories;

namespace Amazon.ProductCatalog.Domain.Tests.Categories
{
    public class TestRepo : IRepository<Category, Guid>
    {
        private List<Category> _categories = new()
        {
            new Category("Furniture"),
            new Category("Electronics"),
            new Category("Shoes"),
        };

        public void Add(Category aggregate)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<Category, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> FilterAsync(Expression<Func<Category, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Category> FilterSingleAsync(Expression<Func<Category, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Category> FilterSingleAsync<TProperty>(Expression<Func<Category, bool>> predicate, Func<IQueryable<Category>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Category, TProperty>> include)
        {
            return Task.FromResult(_categories.FirstOrDefault(predicate.Compile()));
        }

        public Task<IEnumerable<Category>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetByIdAsync<TProperty>(Guid id, Expression<Func<Category, TProperty>> include)
        {
            throw new NotImplementedException();
        }

        public void Remove(Category aggregate)
        {
            throw new NotImplementedException();
        }
    }

    public class CategoriesServiceShould
    {
        [Theory]
        [InlineData("Furniture")]
        [InlineData("Shoes")]
        [InlineData("Electronics")]
        public async Task Throw_When_CategoryName_Exists(string categoryName)
        {
            var repoMoq = new TestRepo();
            var service = new CategoriesService(repoMoq);
            await Assert.ThrowsAnyAsync<Exception>(async () => await service.CreateAsync(categoryName));
        }
    }
}