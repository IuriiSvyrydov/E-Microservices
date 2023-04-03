using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Infrastructure.Data;

namespace Order.Infrastructure
{
    public static class DependencyInjection
    {
     public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration  configuration)
        {
            services.AddDbContext<OrderDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(OrderDbContext).Assembly.FullName)), ServiceLifetime.Singleton);
            return services;
        }
    }

}