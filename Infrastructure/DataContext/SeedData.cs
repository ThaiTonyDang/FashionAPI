using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataContext
{
    public class SeedData
    {
        public static async Task InitializeUserAndRole(AppDbContext context, IServiceProvider servicesProvider)
        {
            using (var scope = servicesProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                context.Database.Migrate();

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

                // created admin role if not existed
                var isAdminRoleExisted = await roleMgr.RoleExistsAsync("Admin");
                if (!isAdminRoleExisted)
                {
                    await roleMgr.CreateAsync(new Role("Admin"));
                }

                // create user admin if not existed
                var adminEmail = "admin@gmail.com";
                var admin = await userMgr.FindByEmailAsync(adminEmail);
                if(admin == null)
                {
                    var adminUser = new User
                    {
                        Email = "admin@gmail.com",
                        UserName = "admin@gmail.com",
                        FirstName = "admin",
                        LastName = "admin",
                    };

                    var result = await userMgr.CreateAsync(adminUser, "abcd@1234");
                    if (result.Succeeded)
                        admin = adminUser;
                }

                var isAdminRole = await userMgr.IsInRoleAsync(admin, "Admin");
                if (!isAdminRole)
                {
                    await userMgr.AddToRoleAsync(admin, "Admin");
                }

                var isUserRoleExisted = await roleMgr.RoleExistsAsync("User");
                if (!isAdminRoleExisted)
                {
                    await roleMgr.CreateAsync(new Role("User"));
                }

                // create user admin if not existed
                var deafaultUserEmail = "user@gmail.com";
                var defaultUser = await userMgr.FindByEmailAsync(deafaultUserEmail);
                if (defaultUser == null)
                {
                    var user = new User
                    {
                        Email = "admin@gmail.com",
                        UserName = "admin@gmail.com",
                        FirstName = "admin",
                        LastName = "admin",
                    };

                    var result = await userMgr.CreateAsync(user, "abcd@1234");
                    if (result.Succeeded)
                        defaultUser = user;
                }

                var isUserRole = await userMgr.IsInRoleAsync(defaultUser, "User");
                if (!isUserRole)
                {
                    await userMgr.AddToRoleAsync(defaultUser, "User");
                }
            }
        }
    }
}
