using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Order.Application.Commands.OrderCreate;
using Order.Application.Responses;
using Order.Domain.Entities;
using Order.Domain.Repositories;

namespace Order.Application.Handlers
{
    public class OrderCreateHandler: IRequestHandler<OrderCreateCommand,OrderResponse>
    {
        private  readonly IOrderRepository _orderRepository;
        private IMapper _mapper;

        public OrderCreateHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderResponse> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
        {
            var ordering = _mapper.Map<Ordering>(request);
            if (ordering is null)
                throw new ApplicationException("Entity could not mapped");
            var order = await _orderRepository.AddAsync(ordering);
            var orderResponse = _mapper.Map<OrderResponse>(order);
            return orderResponse;


        }
    }
}
