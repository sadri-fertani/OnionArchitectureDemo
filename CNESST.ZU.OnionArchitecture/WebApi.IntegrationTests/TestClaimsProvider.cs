using Application.Interfaces.Authentication;
using System.Collections.Generic;
using System.Security.Claims;

namespace WebApi.IntegrationTests
{
    public class TestClaimsProvider
    {
        public IList<Claim> Claims { get; }

        public TestClaimsProvider(IList<Claim> claims)
        {
            Claims = claims;
        }

        public TestClaimsProvider()
        {
            Claims = new List<Claim>();
        }

        public static TestClaimsProvider WithAnonymousUserClaims()
        {
            return null;
        }

        public static TestClaimsProvider WithBasicUserClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, "Test.User.Basic"));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "Test user Basic"));
            provider.Claims.Add(new Claim(ClaimTypes.Email, "test.user@api.basic"));
            provider.Claims.Add(new Claim(ClaimTypes.Role, IRoleProvider.BASIC_USER));

            return provider;
        }

        public static TestClaimsProvider WithAdminUserClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, "Test.User.Admin"));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "Test user Admin"));
            provider.Claims.Add(new Claim(ClaimTypes.Email, "test.user@api.admin"));
            provider.Claims.Add(new Claim(ClaimTypes.Role, IRoleProvider.ADMIN));

            return provider;
        }

        public static TestClaimsProvider WithSuperAdminUserClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, "Test.User.SuperAdmin"));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "Test user SuperAdmin"));
            provider.Claims.Add(new Claim(ClaimTypes.Email, "test.user@api.superadmin"));
            provider.Claims.Add(new Claim(ClaimTypes.Role, IRoleProvider.SUPER_ADMIN));

            return provider;
        }

        public static TestClaimsProvider WithManagerApiUserClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, "Test.User.ManagerApi"));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "Test user ManagerApi"));
            provider.Claims.Add(new Claim(ClaimTypes.Email, "test.user@api.managerapi"));
            provider.Claims.Add(new Claim(ClaimTypes.Role, IRoleProvider.MANAGER_API));

            return provider;
        }
    }
}
