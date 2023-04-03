using E_Microservices.Source.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Microservices.Source.Repositories.Interfaces
{
    public interface IBidRepository
    {
        Task SendBind(Bid bid);
        Task<List<Bid>>GetBidsByAuctionId(string id);
        Task<Bid>GetWinnerBid(string id);
    }
}
