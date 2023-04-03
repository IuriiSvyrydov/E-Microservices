using AutoMapper;
using E_Microservices.Source.Entities;
using EventBusRabbitMQ.Events;

namespace E_Microservices.Source.Mapping
{
    public class SourcingMapping : Profile
    {
        public SourcingMapping()
        {
            CreateMap<OrderCreateEvent,Bid>()
                .ReverseMap();
        }
    }
}
