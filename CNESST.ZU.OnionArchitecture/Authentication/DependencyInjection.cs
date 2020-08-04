using Domain.Settings;
using Application.Interfaces.Authentication;
using Authentication.Provider;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Okta.AspNetCore;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOktaAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            OktaConfig oktaConfig = new OktaConfig();
            configuration.GetSection("ServeroAuth2").Bind(oktaConfig);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
                    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
                    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
                })
                .AddOktaWebApi(new OktaWebApiOptions()
                {
                    OktaDomain = oktaConfig.Domain
                });

            services
                .AddSingleton<IRoleProvider, WebApplicationRoleProvider>();

            services
                .AddSingleton<IClaimsTransformation, RoleAuthorizationTransform>()
                .AddAuthorization();

            return services;
        }
    }
}