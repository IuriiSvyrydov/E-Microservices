using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace E_Microservices.Source.Entities
{
    public class Bid
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string AuctionId { get; set; }
        public string ProductId { get; set; }
        public string SellerUserName { get; set; }
        public decimal Price { get; set; }
        public decimal CreatedAt { get; set; }
    }
}
