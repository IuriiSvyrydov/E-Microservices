using E_Microservices.Source.Data;
using E_Microservices.Source.Data.Interfaces;
using E_Microservices.Source.Repositories;
using E_Microservices.Source.Repositories.Interfaces;
using E_Microservices.Source.Settings;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;


namespace E_Microservices.Source.Extensions
{
    public static class ConfigureExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServicesLayer();
            services.AddSwagger();
            services.AddConfigurationConnection(configuration);
            services.AddRabbitMQ(configuration);
            services.AddAutoMapper();
            return services;
        }
        public static IServiceCollection AddServicesLayer(this IServiceCollection services)
        {
            services.AddScoped<ISourcingContext, SourcingContext>();
            services.AddScoped<IAuctionRepository, AuctionRepository>();
            services.AddScoped<IBidRepository, BidRepository>();
      

            return services;
        }
        public static IServiceCollection AddConfigurationConnection(this  IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SourcingDatabaseSettings>(configuration.GetSection(nameof(SourcingDatabaseSettings)));
            services.AddSingleton<ISourcingDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<SourcingDatabaseSettings>>().Value);
         ;
            return services;
        }
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "E-Microservices.Source",
                    Version = "v1"
                });
            });
            return services;
        }
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IRabbitMQConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = configuration["EventBus:HostName"]
                };
                if (!string.IsNullOrWhiteSpace(configuration["EventBus:UserNane"]))
                {
                    factory.UserName = configuration["EventBus:UserName"];
                }
                if (!string.IsNullOrWhiteSpace(configuration["EventBus:Password"]))
                {
                    factory.UserName = configuration["EventBus:Password"];
                }
                var retryCount = 5;
                if (!string.IsNullOrWhiteSpace(configuration["EventBus:RetryCount"]))
                {
                    retryCount =int.Parse( configuration["EventBus:RetryCount"]);
                }
                return new DefaultRabbitMQConnection(factory, retryCount, logger);

            });
            services.AddSingleton<EventBusRabbitMQProducer>();
            return services;
        }
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            return services;
        }
       
    }
}
