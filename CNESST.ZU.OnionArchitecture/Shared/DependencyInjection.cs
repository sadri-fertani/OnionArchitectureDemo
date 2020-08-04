using Application.Interfaces.Shared;
using Domain.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Services;

namespace Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddShared(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddScoped<IEmailService, EmailService>()
                .Configure<EmailConfig>(configuration.GetSection("Services:SendEmailService"));

            return services;
        }
    }
}