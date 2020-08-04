using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace WebApi.IntegrationTests
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly TestClaimsProvider _claimsProvider;

        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, TestClaimsProvider claimsProvider)
            : base(options, logger, encoder, clock)
        {
            _claimsProvider = claimsProvider;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            AuthenticateResult authenticateResult;

            if (_claimsProvider == null)
                authenticateResult = AuthenticateResult.NoResult();
            else
            {
                var identity = new ClaimsIdentity(_claimsProvider.Claims, WebApplicationFactoryExtensions.AUTHENTICATION_TEST_SCHEME);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, WebApplicationFactoryExtensions.AUTHENTICATION_TEST_SCHEME);

                authenticateResult = AuthenticateResult.Success(ticket);
            }

            return Task.FromResult(authenticateResult);
        }
    }
}
