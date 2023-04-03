using System;
using E_Microservices.Source.Entities;
using MongoDB.Driver;

namespace E_Microservices.Source.Data.Interfaces
{
    public interface ISourcingContext
    {
        IMongoCollection<Auction> Auctions { get;  }
        IMongoCollection<Bid> Binds { get; }
    }
}
