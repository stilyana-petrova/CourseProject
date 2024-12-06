using ArtGallery.Data;
using ArtGallery.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Infrastructure.Data.Infrastructure
{
    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;

            await RoleSeeder(services);
            await SeedAdmin(services);

            var dataCategory = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedCategories(dataCategory);

            var orderStatus=serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedOrderStatus(orderStatus);

            return app;
        }
        private static async Task RoleSeeder(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "Client" };

            IdentityResult roleResult;

            foreach (var role in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        private static async Task SeedAdmin(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            if (await userManager.FindByNameAsync("admin") == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.FirstName = "admin";
                user.LastName = "admin";
                user.UserName = "admin";
                user.Email = "admin";
                user.Address = "admin address";
                user.PhoneNumber = "0888898898";

                var result = await userManager.CreateAsync(user, "admin");
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
        private static void SeedCategories(ApplicationDbContext dataCategory)
        {
            if (dataCategory.Categories.Any())
            {
                return;
            }
            dataCategory.Categories.AddRange(new[]
            {
                new Category {Name="Acrylic paintings"},
                new Category {Name="Watercolor paintings"},
                new Category {Name="Pencil drawings"},
                new Category {Name="Albums"},
                new Category {Name="Cards"},
                new Category {Name="Decorations"},
                new Category {Name="Calendars"},
            });
            dataCategory.SaveChanges();
        }
        private static void SeedOrderStatus(ApplicationDbContext data) 
        {
            if (data.orderStatuses.Any())
            {
                return;
            }
            data.orderStatuses.AddRange(new[]
            {
                new OrderStatus{StatusName="Pending", StatusId=1},
                new OrderStatus{StatusName="Shipped", StatusId=2},
                new OrderStatus{StatusName="Delivered", StatusId=3},
                new OrderStatus{StatusName="Cancelled", StatusId=4},
                new OrderStatus{StatusName="Returned", StatusId=5},
                new OrderStatus{StatusName="Refund", StatusId=6},
            });
            data.SaveChanges();
        }
    }
}
