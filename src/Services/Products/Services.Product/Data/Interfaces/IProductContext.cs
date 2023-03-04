using MongoDB.Driver;

namespace Services.Product.Data.Interfaces
{
    public interface IProductContext
    {
        IMongoCollection<Entities.Product> Products { get;  }
    }
}
