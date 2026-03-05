using System;
using System.Threading.Tasks;
using Ecommerce.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Data
{
    public static class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            var roleMgr = service.GetRequiredService<RoleManager<IdentityRole>>();
            var userMgr = service.GetRequiredService<UserManager<IdentityUser>>();

            // ensure roles
            if (!await roleMgr.RoleExistsAsync(Roles.Admin.ToString()))
            {
                await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            }

            if (!await roleMgr.RoleExistsAsync(Roles.User.ToString()))
            {
                await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));
            }

            // create admin user if missing
            var adminEmail = "admin@gmail.com";
            var admin = await userMgr.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                // Note: this uses a default password for seeding. Replace with a secure value or read from configuration in production.
                var createResult = await userMgr.CreateAsync(admin, "Admin@1234");
                if (!createResult.Succeeded)
                {
                    // optionally handle errors (log/throw). For now, surface the first error.
                    throw new InvalidOperationException($"Failed to create admin user: {createResult.Errors?.GetEnumerator().MoveNext() ?? false}");
                }

                var createdUser = await userMgr.FindByEmailAsync(adminEmail);
                if (createdUser != null && !await userMgr.IsInRoleAsync(createdUser, Roles.Admin.ToString()))
                {
                    await userMgr.AddToRoleAsync(createdUser, Roles.Admin.ToString());
                }
            }
            else
            {
                // ensure existing admin has Admin role
                if (!await userMgr.IsInRoleAsync(admin, Roles.Admin.ToString()))
                {
                    await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
                }
            }
        }
    }
}
