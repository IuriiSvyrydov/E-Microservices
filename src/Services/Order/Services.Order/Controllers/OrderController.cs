using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Order.Application.Commands.OrderCreate;
using Order.Application.Responses;
using Order.Application.Queries;

namespace Services.Order.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController: ControllerBase
    {
        private readonly IMediator _mediaor;
        private readonly ILogger<OrderController>_logger;
        public OrderController(IMediator mediator,ILogger<OrderController>logger)
        {
            _mediaor = mediator;
            _logger = logger;
        }
        [HttpGet("GetOrdersByUserName/{userName}")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>>GetOrdersByUserName(string userName)
        {
            var query = new GetOrderSByUserSellerName(userName);
            var orders = await _mediaor.Send(query);
            if(orders.Count()==decimal.Zero)
                return NotFound(); 
            return Ok(orders);
        }
        [HttpPost]
        [ProducesResponseType(typeof(OrderResponse),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderResponse>> OrderCreate([FromBody] OrderCreateCommand command)
        {
            var  result = await _mediaor.Send(command);
            return Ok(result);
        }
    }
}