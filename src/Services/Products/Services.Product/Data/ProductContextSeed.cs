using System.Collections.Generic;
using MongoDB.Driver;

namespace Services.Product.Data
{
    public static class ProductContextSeed
    {
        public static void SeedData(IMongoCollection<Entities.Product>productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();
            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetConfigureProducts());
            }
        }

        private static IEnumerable<Entities.Product> GetConfigureProducts()
        {
            return new List<Entities.Product>()
            {
                new Entities.Product
                {
                    Name = " IPhone XR",
                    Summary = "Summary of IPhone XR",
                    Description = "Summary of IPhone XR",
                    ImageFile = "product.png",
                    Price = 470.00M,
                    Category = "Mobile Phones"
                }
            };
        }
    }
}
