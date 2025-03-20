using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.Data.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            ctx.Database.MigrateAsync().GetAwaiter().GetResult();
            await SeedAsync(ctx);
        }

        private static async Task SeedAsync(ApplicationDbContext ctx)
        {
            await SeedCustomerAsync(ctx);
            await SeedProductAsync(ctx);
            await SeedOrderAndItemsAsync(ctx);
        }

        private static async Task SeedCustomerAsync(ApplicationDbContext ctx)
        {
            if(!await ctx.Customers.AnyAsync())
            {
                await ctx.Customers.AddRangeAsync(InitialData.Customers);
                await ctx.SaveChangesAsync();
            }
        }
        private static async Task SeedProductAsync(ApplicationDbContext ctx)
        {
            if(!await ctx.Products.AnyAsync())
            {
                await ctx.Products.AddRangeAsync(InitialData.Products);
                await ctx.SaveChangesAsync();
            }
        }
        private static async Task SeedOrderAndItemsAsync(ApplicationDbContext ctx)
        {
            if(!await ctx.Orders.AnyAsync())
            {
                await ctx.Orders.AddRangeAsync(InitialData.OrdersWithItems);
                await ctx.SaveChangesAsync();
            }
        }
    }
}
