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
            new Category("Furniture",null),
            new Category("Electronics",null),
            new Category("Shoes",null),
            new Category("Clothes", null),
        };

        public void Add(Category aggregate)
        {
            _categories.Add(aggregate);
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
            return Task.FromResult(_categories.FirstOrDefault(predicate.Compile()));
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
            return Task.FromResult(_categories.FirstOrDefault(x=>x.Id == id));
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
            await Assert.ThrowsAnyAsync<Exception>(async () => await service.CreateAsync(categoryName, null));
        }

        [Theory]
        [InlineData("Computers")]
        [InlineData("Pants")]
        [InlineData("Airpods")]
        public async Task Create_New_Categories_WithoutParents(string categoryName)
        {
            var repoMoq = new TestRepo();
            var service = new CategoriesService(repoMoq);
            var createdCategory = await service.CreateAsync(categoryName, null);

            Assert.Equal(categoryName, createdCategory.Name);
        }

        [Theory]
        [InlineData("Computers", "Electronics")]
        [InlineData("Pants", "Clothes")]
        [InlineData("Airpods", "Electronics")]
        public async Task Create_New_Categories_WithParents(string categoryName, string parentCategoryName)
        {
            var repoMoq = new TestRepo();
            var service = new CategoriesService(repoMoq);

            var parentCategory = await repoMoq.FilterSingleAsync(c => c.Name == parentCategoryName);

            var createdCategory = await service.CreateAsync(categoryName, parentCategory.Id);

            Assert.Equal(categoryName, createdCategory.Name);
            Assert.Equal(parentCategoryName, parentCategory.Name);
        }
    }
}