using E_Microservices.Source.Entities;
using E_Microservices.Source.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace E_Microservices.Source.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidRepository _bindRepository;

        public BidController(IBidRepository bindRepository)
        {
            _bindRepository = bindRepository;
        }
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> SendBind([FromBody]Bid bid)
        {
            await _bindRepository.SendBind(bid);
            return Ok();
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Bid>),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Bid>>>GetBidBuAuctionId(string id)
        {
            var bids = await _bindRepository.GetBidsByAuctionId(id);
            return Ok(bids);
        }
        [HttpGet("GetWinnerBid")]
        [ProducesResponseType(typeof(Bid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult>GetWinnerBid(string id)
        {
            return Ok(await _bindRepository.GetWinnerBid(id));
        }
    }
}
