using E_Microservices.Source.Data.Interfaces;
using E_Microservices.Source.Entities;
using E_Microservices.Source.Repositories.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Microservices.Source.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly ISourcingContext _context;
        public BidRepository(ISourcingContext context)
        {
            _context = context;
        }
        public async Task<List<Bid>> GetBidsByAuctionId(string id)
        {
            FilterDefinition<Bid> filter = Builders<Bid>.Filter.Eq(a => a.AuctionId, id);
            List<Bid> result = await _context.Binds.Find(filter)
                .ToListAsync();
            result = result.OrderByDescending(a => a.CreatedAt)
                                            .GroupBy(a => a.SellerUserName)
                                            .Select(a => new Bid
                                            {
                                                AuctionId = a.FirstOrDefault().AuctionId,
                                                Price = a.FirstOrDefault().Price,
                                                CreatedAt = a.FirstOrDefault().CreatedAt,
                                                SellerUserName = a.FirstOrDefault().SellerUserName,
                                                ProductId = a.FirstOrDefault().ProductId,
                                                Id = a.FirstOrDefault().Id
                                            }).ToList();
            return result;
        }

        public async Task<Bid> GetWinnerBid(string id)
        {
            List<Bid>bids = await GetBidsByAuctionId(id);
            return bids.OrderByDescending(a=>a.Price).FirstOrDefault();
        }

        public async Task SendBind(Bid bid)
        {
          await  _context.Binds.InsertOneAsync(bid);
        }
    }
}
