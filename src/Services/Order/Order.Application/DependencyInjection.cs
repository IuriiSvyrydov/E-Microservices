using System.Reflection;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Mapper;
using Order.Domain.Repositories;
using Order.Domain.Repositories.Base;
using Order.Infrastructure.Repositories;
using Order.Infrastructure.Repositories.Base;

namespace Order.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this  IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped(typeof(IRepository<>),typeof(Repository<>));

            #region MappingConfig

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<OrderMapping>();
            });
            

            #endregion

            return services;
        }
    }
}
