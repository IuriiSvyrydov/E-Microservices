using MongoDB.Driver;
using Services.Product.Data.Interfaces;
using Services.Product.Settings;

namespace Services.Product.Data
{
    public class ProductContext: IProductContext
    {
       // private  readonly  IProductDatabaseSettings _databaseSettings;
        public IMongoCollection<Entities.Product> Products { get;  }

        public ProductContext(IProductDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionStrings);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            Products = database.GetCollection<Entities.Product>(databaseSettings.CollectionName);
            ProductContextSeed.SeedData(Products);
        }
    }
}
