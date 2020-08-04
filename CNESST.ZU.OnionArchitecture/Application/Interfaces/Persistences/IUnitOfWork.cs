using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Application.Interfaces.Persistences
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : BaseEntity;

        Task<int> CommitAsync();
    }

    public interface IUnitOfWork<out TContext> : IUnitOfWork where TContext : DbContext, IApplicationDbContext
    {
        TContext Context { get; }
    }
}
