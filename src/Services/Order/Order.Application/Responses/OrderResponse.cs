using System;

namespace Order.Application.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public string AuctionId { get; set; }
        public string SellerUserName { get; set; }
        public string ProductId { get; set; }
        public decimal UnitOfPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
