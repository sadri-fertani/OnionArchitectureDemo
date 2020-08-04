using Domain.Entities;
using Persistence.Context;

namespace WebApi.IntegrationTests
{
    public static class SeedData
    {
        public static void Populate(this ApplicationDbContext context)
        {
            context.Products.Add(
                new Product
                {
                    Name = "iPhone XR",
                    Description = "Description for iPhone XR",
                    Price = 410
                });

            context.Products.Add(
                new Product
                {
                    Name = "One Plus 7 Pro",
                    Description = "Description for One Plus 7 pro",
                    Price = 420
                });

            context.Products.Add(
                new Product
                {
                    Name = "MacBook Pro",
                    Description = "Description for MacBook Pro",
                    Price = 850
                });

            context.SaveChanges();
        }
    }
}
