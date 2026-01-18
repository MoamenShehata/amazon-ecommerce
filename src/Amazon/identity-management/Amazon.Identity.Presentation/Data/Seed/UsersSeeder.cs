using Amazon.Identity.Presentation.Models;
using Microsoft.AspNetCore.Identity;

namespace Amazon.Identity.Presentation.Data.Seed
{
    public static class UsersSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var adminDefaultEmail = "admin@amazon.com";
            var adminDefaultPassword = "a@A123456789";

            var adminUser = await userManager.FindByEmailAsync(adminDefaultEmail);
            if (adminUser is null)
            {
                var adminUserToCreate = new ApplicationUser
                {
                    Email = adminDefaultEmail,
                    UserName = adminDefaultEmail,
                    EmailConfirmed = true,
                };

                var adminUserCreationResult = await userManager.CreateAsync(adminUserToCreate, adminDefaultPassword);
                if (!adminUserCreationResult.Succeeded)
                    throw new Exception(string.Join(", ",
                        adminUserCreationResult.Errors.Select(e => e.Description)));

                await userManager.AddToRoleAsync(adminUserToCreate, "Admin");
            }
        }
    }
}