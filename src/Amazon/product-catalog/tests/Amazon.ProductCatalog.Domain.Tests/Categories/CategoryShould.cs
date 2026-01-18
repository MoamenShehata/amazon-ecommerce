using Amazon.ProductCatalog.Domain.Categories;

namespace Amazon.ProductCatalog.Domain.Tests.Categories
{
    public class CategoryShould
    {
        [Fact]
        public void Render_FullName_Correctly()
        {
            var android = new Category("android", new Category("mobiles", new Category("electronics", null)));

            Assert.Equal("android,mobiles,electronics", android.FullName);
        }
    }
}