using E_Microservices.Source.Data.Interfaces;
using E_Microservices.Source.Entities;
using E_Microservices.Source.Settings;
using MongoDB.Driver;

namespace E_Microservices.Source.Data
{
    public class SourcingContext: ISourcingContext
    {
        public IMongoCollection<Auction> Auctions { get; }
        public IMongoCollection<Bid> Binds { get; }

        public SourcingContext(ISourcingDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            Auctions = database.GetCollection<Auction>(nameof(Auction));
            Binds = database.GetCollection<Bid>(nameof(Bid));
            SourcingContextSeed.SeedData(Auctions);
        }

    }
}
