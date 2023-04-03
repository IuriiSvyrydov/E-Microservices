

using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Services.Product.Data.Interfaces;
using Services.Product.Repositories.Interfaces;

namespace Services.Product.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly IProductContext _context;

        public ProductRepository(IProductContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Entities.Product>> GetProducts()
        {
            return await _context.Products.Find(p => true).ToListAsync();
        }

        public async Task<Entities.Product> GetProduct(string id)
        {
            return await _context.Products.Find(p=>p.Id==id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Entities.Product>> GetProductByName(string name)
        {
            var filter = Builders<Entities.Product>.Filter.Eq(p => p.Name, name);
            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Entities.Product>> GetProductByCategory(string categoryName)
        {
            var filter = Builders<Entities.Product>.Filter.Eq(p => p.Category, categoryName);
            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task Create(Entities.Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> Update(Entities.Product product)
        {
            var updateProduct = await _context.Products.ReplaceOneAsync(filter: g => g.Id == product.Id,
                replacement: product);
            return updateProduct.IsAcknowledged && updateProduct.MatchedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Entities.Product>.Filter.Eq(x => x.Id, id);
            DeleteResult result = await _context.Products.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;

        }
    }
}
