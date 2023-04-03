
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Order.Infrastructure.Data
{
    public class OrderDesignTimeFactory : IDesignTimeDbContextFactory<OrderDbContext>
    {
        public OrderDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();
            optionsBuilder.UseSqlServer("Server=REVISION-PC;Database=orderDb;Trusted_Connection=True;Integrated Security=true");

            return new OrderDbContext(optionsBuilder.Options);
        }
    }
}
