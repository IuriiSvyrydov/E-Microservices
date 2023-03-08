using E_Microservices.Source.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace E_Microservices.Source.Extensions
{
    public static class ConfigureExtensions
    {
        public static IServiceCollection AddConfigurationConnection(this  IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SourcingDatabaseSettings>(configuration.GetSection(nameof(SourcingDatabaseSettings)));
            services.AddSingleton<ISourcingDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<SourcingDatabaseSettings>>().Value);
            return services;
        }

    }
}
