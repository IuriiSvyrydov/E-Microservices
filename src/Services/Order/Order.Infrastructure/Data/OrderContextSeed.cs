

using System;
using System.Collections.Generic;
using Order.Domain.Entities;

namespace Order.Infrastructure.Data
{
    public static class OrderContextSeed
    {
        public static IEnumerable<Ordering> AddSeed()
        {
            return new List<Ordering>()
            {
                new Ordering
                {

                    AuctionId = Guid.NewGuid().ToString(),
                    ProductId = Guid.NewGuid().ToString(),
                    SellerUserName = "test@test.com",
                    UnitPrice = 10,
                    TotalPrice = 1000,
                    CreateAt = DateTime.Now
                }
            };
        }
    }
}
