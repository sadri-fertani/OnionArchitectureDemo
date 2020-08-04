using Application.Interfaces.Persistences;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

        public Repository(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<T> GetAsync(long id)
        {
            var query = _unitOfWork.Context.Set<T>().AsQueryable();
            IncludeChildren(ref query);

            return await query.FirstOrDefaultAsync<T>(x => x.Id == id);
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            var query = _unitOfWork.Context.Set<T>().AsQueryable();
            IncludeChildren(ref query);

            return await query.ToArrayAsync<T>();
        }

        public IQueryable<T> GetQueryable()
        {
            return _unitOfWork.Context.Set<T>().AsQueryable();
        }

        public void Add(T entity)
        {
            _unitOfWork.Context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            T existing = _unitOfWork.Context.Set<T>().Find(entity.Id);
            if (existing != null) _unitOfWork.Context.Set<T>().Remove(existing);
        }

        public void Update(T entity)
        {
            T existing = _unitOfWork.Context.Set<T>().Find(entity.Id);

            if (existing != null)
            {
                _unitOfWork.Context.Entry(existing).State = EntityState.Modified;
                _unitOfWork.Context.Entry(existing).CurrentValues.SetValues(entity);
            }
        }

        private void IncludeChildren(ref IQueryable<T> query)
        {
            string includeRelations = null;

            switch (typeof(T).Name)
            {
                //case nameof(ShoppingCart):
                //    includeRelations = "Commands.Product.ProductDetails";
                //    break;
                //case nameof(Command):
                //    includeRelations = "Product.ProductDetails";
                //    break;
                //case nameof(Product):
                //    includeRelations = "ProductDetails";
                //    break;
                default:
                    break;
            }

            if (includeRelations != null)
                query = query.Include(includeRelations);
        }
    }
}
