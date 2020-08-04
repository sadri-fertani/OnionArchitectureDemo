using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CNESST.ZU.AppDemo.Services
{
    public class AuthorityService : IAuthorityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthorityService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public async Task<string> GetTokenAsync()
        {
            return await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        }
    }
}
