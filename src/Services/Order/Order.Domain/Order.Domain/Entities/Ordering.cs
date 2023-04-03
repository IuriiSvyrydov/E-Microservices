using System;


namespace Order.Domain.Entities
{
    public class Ordering: BaseEntity
    {
        public string AuctionId { get; set; }
        public string SellerUserName { get; set; }
        public string ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
