using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using CNESST.AppWebDem.Provider.Role;
using CNESST.ZU.AppDemo.Services;
using CNESST.ZU.AppDemo.Services.ProductsAPI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;

namespace CNESST.AppWebDem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddRefitClient<IProductService>()
                .ConfigureHttpClient(c => 
                {
                    ProductServiceSettings productServiceSettings = new ProductServiceSettings();
                    Configuration.GetSection("Services:ProductService").Bind(productServiceSettings);

                    c.BaseAddress = new Uri(productServiceSettings.BaseUrl);
                });

            services
                .AddSingleton<IAuthorityService, AuthorityService>();

            services
                .AddAuthentication(options =>
                {
                    // If an authentication cookie is present, use it to get authentication information
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    // If authentication is required, and no cookie is present, use oAuth2Custom (configured below) to sign in
                    options.DefaultChallengeScheme = "oAuth2Custom";

                })
                .AddCookie() // cookie authentication middleware first
                .AddOAuth("oAuth2Custom", options =>
                {
                    OktaConfig oktaConfig = new OktaConfig();
                    Configuration.GetSection("ServeroAuth2").Bind(oktaConfig);

                    // When a user needs to sign in, they will be redirected to the authorize endpoint
                    options.AuthorizationEndpoint = oktaConfig.AuthorizationEndpoint;
                    // OAuth server is OpenID compliant, so request the standard openid
                    // scopes when redirecting to the authorization endpoint
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    // After the user signs in, an authorization code will be sent to a callback
                    // in this app. The OAuth middleware will intercept it
                    options.CallbackPath = new PathString("/authorization-code/callback");
                    // The OAuth middleware will send the ClientId, ClientSecret, and the
                    // authorization code to the token endpoint, and get an access token in return
                    options.ClientId = oktaConfig.ClientId;
                    options.ClientSecret = oktaConfig.ClientSecret;
                    options.TokenEndpoint = oktaConfig.TokenEndpoint;
                    // Below we call the userinfo endpoint to get information about the user
                    options.UserInformationEndpoint = oktaConfig.UserInformationEndpoint;
                    // Describe how to map the user info we receive to user claims
                    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "given_name");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                    options.ClaimActions.MapJsonKey("FamilyName", "family_name");
                    options.ClaimActions.MapJsonKey("Nickname", "nickname");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Country, "locale");

                    options.SaveTokens = true;

                    options.Events = new OAuthEvents
                    {
                        OnCreatingTicket = async context =>
                        {
                            // Get user info from the userinfo endpoint and use it to populate user claims
                            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);

                            response.EnsureSuccessStatusCode();
                            var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                            context.RunClaimActions(user.RootElement);

                            List<AuthenticationToken> tokens = context.Properties.GetTokens().ToList();

                            tokens.Add(new AuthenticationToken
                            {
                                Name = "TicketCreated",
                                Value = DateTime.UtcNow.ToString()
                            });

                            tokens.Add(new AuthenticationToken
                            {
                                Name = "bizzzz",
                                Value = "3.02.35"
                            });

                            context.Properties.StoreTokens(tokens);
                        }
                    };
                });

            services
                .AddRoleAuthorization<WebApplicationRoleProvider>()
                .AddAuthorization();

            //services
            //    .Configure<ProductServiceSettings>(Configuration.GetSection("Services:ProductService"));

            services
                .AddHttpContextAccessor();

            //services
            //    .AddScoped<IProductService, ProductService>();

            services
                .AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app
                .UseAuthentication();

            app
                .UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
