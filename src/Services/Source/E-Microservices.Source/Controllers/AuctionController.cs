using AutoMapper;
using E_Microservices.Source.Entities;
using E_Microservices.Source.Repositories.Interfaces;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace E_Microservices.Source.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {

        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly ILogger<AuctionController> _logger;
        private EventBusRabbitMQProducer _eventBus;
        private IMapper _mapper;
        public AuctionController(IAuctionRepository auctionRepository,IBidRepository bidRepository,IMapper mapper, EventBusRabbitMQProducer eventBus,
            ILogger<AuctionController> logger)
        {
            _auctionRepository = auctionRepository;
            _bidRepository = bidRepository;
            _eventBus = eventBus;
            _mapper = mapper;
            _logger = logger;

        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Auction>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAuctions()
        {
            var auctions = await _auctionRepository.GetAuctions();
            return Ok(auctions);
        }
        [HttpGet("{id:length(24)}", Name = "GetAuction")]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Auction>> GetAuction(string id)
        {
            var auction = await _auctionRepository.GetAuction(id);
            if (auction is null)
            {
                _logger.LogError($"Auction with id: {id} has not found");
                return NotFound();
            }

            return Ok(auction);
        }
        [HttpPost]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Auction>> CreateAuction([FromBody] Auction auction)
        {
            await _auctionRepository.Create(auction);
            return CreatedAtRoute("GetAuction", new { id = auction.Id }, auction);
        }
        [HttpPut]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> UpdateAuction([FromBody] Auction auction)
        {
            return Ok(await _auctionRepository.Update(auction));
        }
        [HttpDelete("{id:length(24)}", Name = "GetAuction")]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Auction>> DeleteAuction(string id)
        {
            return Ok(await _auctionRepository.Delete(id));
        }
        [HttpPost("CompleteAuction")]
        [ProducesResponseType( (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult>CompleteAuction(string id)
        {
            var auction  = await _auctionRepository.GetAuction(id);
            if (auction == null)
                return NotFound();
            if (auction.Status!=(int)Status.Active)
            {
                _logger.LogError("Auction can not be completed");
                return BadRequest();
            }
            var bid = await _bidRepository.GetWinnerBid(id);
            if (bid == null) return NotFound();

            OrderCreateEvent eventMessage = _mapper.Map<OrderCreateEvent>(bid);
            return Ok(eventMessage);
        }
        [HttpPost("TestEvent")]
        public ActionResult<OrderCreateEvent>TestEvent()
        {
            OrderCreateEvent eventMessage = new OrderCreateEvent();
            eventMessage.AuctionId = "dummy1";
            eventMessage.ProductId = "product_1";
            eventMessage.Price = 10;
            eventMessage.Quantity = 100;
            eventMessage.SellerUserName = "twst@test.com";
            try
            {
                _eventBus.Publish(EventBusConstants.OrderEventBus, eventMessage);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "ERROR Publish integration event:{EventId} from {AppName}", eventMessage.Id, "Sourcing");
                throw;
            }
            return Accepted(eventMessage);
        }
    }
}