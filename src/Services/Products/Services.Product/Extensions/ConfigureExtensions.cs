using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Services.Product.Data;
using Services.Product.Data.Interfaces;
using Services.Product.Repositories;
using Services.Product.Repositories.Interfaces;
using Services.Product.Settings;

namespace Services.Product.Extensions
{
    public static class ConfigureExtensions
    {
        public static IServiceCollection AddConfigureExtensions(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<ProductDatabaseSettings>(configuration.GetSection(nameof(ProductDatabaseSettings)));
            services.AddSingleton<IProductDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<ProductDatabaseSettings>>().Value);
            services.AddApplicationExtensions();
            return services;
        }

        public static IServiceCollection AddApplicationExtensions(this IServiceCollection services)
        {
            services.AddScoped<IProductContext, ProductContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
       
    }
}
