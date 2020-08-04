using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Interfaces.Authentication
{
    public interface IRoleProvider
    {
        const string MANAGER_API = "ManagerAPI";
        const string SUPER_ADMIN = "SuperAdmin";
        const string ADMIN = "Admin";
        const string BASIC_USER = "BasicUser";

        Task<IList<string>> GetUserRolesAsync(ClaimsIdentity identity);
    }
}
