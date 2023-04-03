using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;
using Order.Domain.Repositories;
using Order.Infrastructure.Data;
using Order.Infrastructure.Repositories.Base;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Ordering>,IOrderRepository
    {
        private readonly OrderDbContext _context;
        public OrderRepository(OrderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ordering>> GetOrderBySallerName(string userName)
        {
            var sallerList = await _context.Orders.Where(o => o.SellerUserName == userName)
                .ToListAsync();
            return sallerList;
        }
    }
}
