using Microsoft.AspNetCore.Identity;

namespace Amazon.Identity.Presentation.Data.Seed;

public static class RolesSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var rolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var customersRole = await rolesManager.FindByNameAsync(RoleNames.Customer);
        if (customersRole is not null)
            return;

        await rolesManager.CreateAsync(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = RoleNames.Customer, NormalizedName = RoleNames.Customer.ToUpper() });

    }
}