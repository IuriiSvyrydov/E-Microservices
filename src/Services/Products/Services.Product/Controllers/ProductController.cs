using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Product.Repositories;
using Services.Product.Repositories.Interfaces;

namespace Services.Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private  readonly  IProductRepository _productRepository;
        private readonly ILogger<ProductRepository> _logger;

        public ProductController(IProductRepository productRepository,
            ILogger<ProductRepository> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Entities.Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Entities.Product>>> GetProducts()
        {
            return Ok (await _productRepository.GetProducts());
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Entities.Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Entities.Product>> GetProduct(string id)
        {
            var findProduct = await _productRepository.GetProduct(id);
            if (findProduct==null)
            {
                _logger.LogError($"product with id {id} is not found");
            }
            return Ok(findProduct);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Entities.Product), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Entities.Product>> CreateProduct([FromBody] Entities.Product product)
        {
            await _productRepository.Create(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Entities.Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Entities.Product>> UpdateProduct([FromBody] Entities.Product product)
        {
     
            return Ok(await _productRepository.Update(product));
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(Entities.Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Entities.Product>> DeleteProduct(string id)
        {
            var findProduct = await _productRepository.GetProduct(id);
            if (findProduct == null)
            {
                _logger.LogError($"product with id {id} is not found");
            }

           var  productToDelete = await _productRepository.Delete(id);
           return Ok(productToDelete);
        }

    }
}
