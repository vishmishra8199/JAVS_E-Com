using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JWT_Token_Example.Inventory.InventoryModels;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string id { get; set; }

    public string ProductName { get; set; }



    public string Category { get; set; }


    public List<Items> items { get; set; }

    public int Quantity { get; set; }


    public string Status { get; set; }
}