using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Application.Queries;
using Order.Application.Responses;
using Order.Domain.Repositories;

namespace Order.Application.Handlers
{
    public class GetOrdersByUserNameHandler: IRequestHandler<GetOrderSByUserSellerName,IEnumerable<OrderResponse>>
    {
        private  readonly  IOrderRepository _orderRepository;
        private IMapper _mapper;
        public GetOrdersByUserNameHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderResponse>> Handle(GetOrderSByUserSellerName request,
            CancellationToken cancellationToken)
        {
            var query = await _orderRepository.GetOrderBySallerName(request.UserName);
            var response = _mapper.Map<IEnumerable<OrderResponse>>(query);
            return response;
           
        }
    }
}
