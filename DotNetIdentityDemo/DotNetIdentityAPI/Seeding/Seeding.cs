using DotNetIdentityAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotNetIdentityAPI.Seeding
{
    public class Seeding
    {
        public static async Task SeedDataAndApplyPendingMigrationsAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (dbContext.Database.GetPendingMigrations().Count() > 0)
                {
                     dbContext.Database.Migrate(); 
                }

                if (!dbContext.Products.Any())
                {
                    dbContext.Products.AddRange(
                        new Product { ProductName = "product1", Price = 10.99m, },
                        new Product { ProductName = "product2", Price = 10.99m, },
                        new Product { ProductName = "product3", Price = 10.99m, }
                    );
                    dbContext.SaveChanges();
                }

                if (!dbContext.Orders.Any())
                {
                    dbContext.Orders.AddRange(
                        new Order { OrderDate = DateTime.Now, OrderAmount = 200.99m, CustomerEmail = "customer1@gmail.com", ProductId = dbContext.Products.FirstOrDefault(p => p.ProductName == "product1").ProductId }
                    );
                    dbContext.SaveChanges();
                }

                if (roleManager.FindByNameAsync("OrderCreator").Result == null)
                {
                    await roleManager.CreateAsync(new IdentityRole() { Name = "OrderCreator" });
                }

                if (roleManager.FindByNameAsync("ProductCreator").Result == null)
                {
                    await roleManager.CreateAsync(new IdentityRole() { Name = "ProductCreator" });
                }

                if (userManager.FindByEmailAsync("ordercreator@gmail.com").Result == null)
                {
                    await userManager.CreateAsync(new IdentityUser() { Email = "ordercreator@gmail.com", UserName = "ordercreator@gmail.com" }, "Abc@123456hfd.");
                    await userManager.AddToRoleAsync(userManager.FindByEmailAsync("ordercreator@gmail.com").Result, "OrderCreator");
                }

                if (userManager.FindByEmailAsync("productcreator@gmail.com").Result == null)
                {
                    await userManager.CreateAsync(new IdentityUser() { Email = "productcreator@gmail.com", UserName = "productcreator@gmail.com" }, "Abc@123456hfd.");
                    await userManager.AddToRoleAsync(userManager.FindByEmailAsync("productcreator@gmail.com").Result, "ProductCreator");
                }



            }
        }
    }
}
