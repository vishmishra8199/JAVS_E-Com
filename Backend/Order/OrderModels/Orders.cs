using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JWT_Token_Example.Order.OrderModels;

public class Orders
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? id { get; set; }

    public string BillingAddressId { get; set; }

    public string BuyerId { get; set; }

    public List<OrderItems> orders { get; set; }

    public long TotalAmount { get; set; }

    public long TotalQuantity { get; set; }

}