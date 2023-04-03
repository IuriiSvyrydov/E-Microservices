using Order.Domain.Entities;
using Order.Domain.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Repositories
{
    public interface IOrderRepository: IRepository<Ordering>
    {
        Task<IEnumerable<Ordering>> GetOrderBySallerName(string userName);
    }
}
