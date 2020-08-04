using Application.Interfaces.Persistences;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Product>()
                .ToTable("Product")
                .Property(p => p.Price)
                .HasColumnType("decimal(10, 2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}