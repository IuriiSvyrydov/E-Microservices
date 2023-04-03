using E_Microservices.Source.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace E_Microservices.Source.Data.Interfaces
{
    public class SourcingContextSeed
    {
        public static void SeedData(IMongoCollection<Auction>auctionCollection)
        {
            bool exist =auctionCollection.Find(p=>true).Any();
            if (!exist) 
            {
                auctionCollection.InsertManyAsync(GetPreconfiguredAuctions());
            }
        }

        private static IEnumerable<Auction> GetPreconfiguredAuctions()
        {
            return new List<Auction>()
           {
               new Auction()
               {
                   Name = "Auction 1",
                   Description = "Auction Desc 1",
                   CreatedAt = DateTime.Now,
                   StartedAt = DateTime.Now,
                   FinishedAt = DateTime.Now.AddDays(10),
                   ProductId = "4E4EDAB3-E3E0-4144-9E24-12077777A5F3",
                   IncludedSellers = new List<string>()
                   {
                       "saler1@test.com",
                       "saler2@test.com",
                       "saler3@test.com"
                   },
                   Quantity = 5,
                   Status = (int)Status.Active

               }
           };
        }
    }
}
