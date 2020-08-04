using System.Threading.Tasks;

namespace CNESST.ZU.AppDemo.Services
{
    public interface IAuthorityService
    {
        Task<string> GetTokenAsync();
    }
}
