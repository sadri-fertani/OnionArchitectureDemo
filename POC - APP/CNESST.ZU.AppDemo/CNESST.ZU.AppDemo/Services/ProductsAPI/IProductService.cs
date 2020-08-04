using CNESST.ZU.AppDemo.Models;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CNESST.ZU.AppDemo.Services.ProductsAPI
{
    public interface IProductService
    {
        [Get("/api/product")]
        Task<List<ProductModel>> GetAllAsync([Header("Authorization")] string authorization);

        [Get("/api/product/{id}")]
        Task<ProductModel> GetOneAsync(long id);
    }
}
