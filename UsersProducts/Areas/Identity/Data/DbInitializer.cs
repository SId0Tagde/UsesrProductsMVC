using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsersProducts.Data;

namespace UsersProducts.Areas.Identity.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<UsersProductsContext>();
            context.Database.Migrate();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
