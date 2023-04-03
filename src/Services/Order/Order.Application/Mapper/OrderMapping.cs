using AutoMapper;
using Order.Application.Commands.OrderCreate;
using Order.Application.Responses;
using Order.Domain.Entities;

namespace Order.Application.Mapper
{
    public class OrderMapping :Profile
    {
        public OrderMapping()
        {
            CreateMap<Ordering, OrderCreateCommand>()
                .ReverseMap();
            CreateMap<Ordering, OrderResponse>()
                .ReverseMap();
            //CreateProjection<Ordering, OrderResponse>();
        }
    }
}
