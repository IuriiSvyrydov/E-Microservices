using System.Collections.Generic;
using System.Threading.Tasks;
using Product = Services.Product.Entities;

namespace Services.Product.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Entities.Product>> GetProducts();
        Task<Entities.Product> GetProduct(string id);
        Task<IEnumerable<Entities.Product>> GetProductByName(string name);
        Task<IEnumerable<Entities.Product>>GetProductByCategory(string categoryName);


        Task Create(Entities.Product product);
        Task<bool> Update(Entities.Product product);
        Task<bool> Delete(string id);
    }
}
