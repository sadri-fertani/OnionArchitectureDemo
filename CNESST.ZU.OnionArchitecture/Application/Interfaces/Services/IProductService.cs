using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IProductService : IProductReadableService, IProductWritableService
    {

    }

    public interface IProductReadableService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> GetProduct(long id);
    }

    public interface IProductWritableService
    {
        Task<ProductDto> InsertProduct(ProductDto dto);
        Task UpdateProduct(ProductDto dto);
        Task DeleteProduct(long id);
    }
}
