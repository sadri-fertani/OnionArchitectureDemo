using Application.Interfaces.Services;
using Application.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddAutoMapper(typeof(ApplicationProfile));

            services
                .AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
