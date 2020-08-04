using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces.Persistences
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; set; }
    }
}
