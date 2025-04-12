using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ERMS.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            try
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                string[] roles = { "Admin", "Manager", "Employee" };

                // Create roles if they don't exist.
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        var result = await roleManager.CreateAsync(new IdentityRole(role));
                        if (result.Succeeded)
                        {
                            Console.WriteLine($"Created role: {role}");
                        }
                        else
                        {
                            Console.WriteLine($"Error creating role {role}: {string.Join(", ", result.Errors)}");
                        }
                    }
                }

                // Seed Admin User
                await CreateUserIfNotExists(userManager, "admin@example.com", "Admin@123", "Admin");

                // Seed Manager User
                await CreateUserIfNotExists(userManager, "manager@example.com", "Manager@123", "Manager");

                // Seed Employee User
                await CreateUserIfNotExists(userManager, "employee@example.com", "Employee@123", "Employee");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during DB seeding: {ex.Message}");
            }
        }

        private static async Task CreateUserIfNotExists(UserManager<IdentityUser> userManager, string email, string password, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser { UserName = email, Email = email };
                var createResult = await userManager.CreateAsync(user, password);
                if (createResult.Succeeded)
                {
                    var addRoleResult = await userManager.AddToRoleAsync(user, role);
                    if (addRoleResult.Succeeded)
                    {
                        Console.WriteLine($"Created user {email} and assigned role '{role}'.");
                    }
                    else
                    {
                        Console.WriteLine($"User {email} created, but failed to add to role '{role}': {string.Join(", ", addRoleResult.Errors)}");
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to create user {email}: {string.Join(", ", createResult.Errors)}");
                }
            }
            else
            {
                Console.WriteLine($"User {email} already exists.");
            }
        }
    }
}
