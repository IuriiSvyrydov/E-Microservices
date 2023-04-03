using System.Collections.Generic;
using System.Linq;
using MediatR;
using Order.Application.Responses;

namespace Order.Application.Queries
{
    public class GetOrderSByUserSellerName: IRequest<IEnumerable<OrderResponse>>
    {
        public GetOrderSByUserSellerName(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; set; }
    }
}
