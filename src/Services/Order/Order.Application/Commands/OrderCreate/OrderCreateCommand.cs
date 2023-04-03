
using MediatR;
using Order.Application.Responses;
using Order.Domain.Entities;
using System;

namespace Order.Application.Commands.OrderCreate
{
    public class OrderCreateCommand: IRequest<OrderResponse>
    {
        public string AuctionId { get; set; }
        public string SellerUserName { get; set; }
        public string ProductId { get; set; }
        public decimal UnitOfPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
