using Amazon.ProductCatalog.Domain.Categories;

namespace Amazon.ProductCatalog.Domain.Tests.Categories
{

    public class CategoriesServiceShould
    {
        private static CategoriesService _service;
        private static TestRepo _testRepo;

        public CategoriesServiceShould()
        {
            _testRepo = new TestRepo();
            _service = new CategoriesService(_testRepo);
        }

        [Theory]
        [InlineData("Furniture")]
        [InlineData("Shoes")]
        [InlineData("Electronics")]
        public async Task Throw_When_CategoryName_Exists(string categoryName)
        {
            await Assert.ThrowsAnyAsync<Exception>(async () => await _service.CreateAsync(categoryName, null));
        }

        [Theory]
        [InlineData("Computers")]
        [InlineData("Pants")]
        [InlineData("Airpods")]
        public async Task Create_New_Categories_WithoutParents(string categoryName)
        {
            var createdCategoryResult = await _service.CreateAsync(categoryName, null);

            Assert.Equal(categoryName, createdCategoryResult.Value.Name);
        }

        [Theory]
        [InlineData("Computers", "Electronics")]
        [InlineData("Pants", "Clothes")]
        [InlineData("Airpods", "Electronics")]
        public async Task Create_New_Categories_WithParents(string categoryName, string parentCategoryName)
        {
            var parentCategory = await _testRepo.FilterSingleAsync(c => c.Name == parentCategoryName);

            var createdCategory = await _service.CreateAsync(categoryName, parentCategory.Id);

            Assert.Equal(categoryName, createdCategory.Value.Name);
            Assert.Equal(parentCategoryName, parentCategory.Name);
        }

        [Fact]
        public async Task Throw_To_Update_Non_Existing()
        {
            await Assert.ThrowsAnyAsync<Exception>(async () => await _service.UpdateAsync(Guid.NewGuid(), "newCategoryName", null));
        }

        //to do we need to get a category in the service
        [Theory]
        [InlineData("Furniture")]
        [InlineData("Shoes")]
        [InlineData("Electronics")]
        public async Task Throw_To_Update_Using_Existing_Category_Names(string categoryName)
        {
            var categoryToUpdate = await _testRepo.FilterSingleAsync(c => c.Name == categoryName);

            await Assert.ThrowsAnyAsync<Exception>(async () => await _service.UpdateAsync(categoryToUpdate.Id, categoryName, null));
        }

        [Theory]
        [InlineData("Furniture", "Furniture-")]
        [InlineData("Shoes", "Shoes-")]
        [InlineData("Electronics", "Electronics-")]
        public async Task Update(string categoryName, string newName)
        {
            var categoryToUpdate = await _testRepo.FilterSingleAsync(c => c.Name == categoryName);

            await _service.UpdateAsync(categoryToUpdate.Id, newName, null);
        }
    }
}