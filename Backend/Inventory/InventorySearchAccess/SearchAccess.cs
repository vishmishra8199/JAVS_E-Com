using JWT_Token_Example.Inventory.InventoryModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace JWT_Token_Example.Inventory.InventorySearchAccess;

public class SearchAccess
{
    private const string ConnectionString = "mongodb+srv://vishalmishra:Kunal8199@cluster0.hqijrs7.mongodb.net/?retryWrites=true&w=majority";
        // add connection string here
        private const string DatabaseName = "SUJITH_DB";
        // add database name here
        private const string inventory = "inventory";


        public readonly IMongoCollection<Product> productsCollection;

        //    "ConnectionURL": "mongodb+srv://vishalmishra:Kunal8199@cluster0.hqijrs7.mongodb.net/?retryWrites=true&w=majority",
        //"DatabaseName": "SUJITH_DB",
        //"CollectionName": "inventory"

        // add the name of collection here
        //private const string UserCollection = "users";
        //private const string ChoreHistoryCollection = "chore_history";


        // USE COMMON FILE FOR MONGO CONNECTION

        public SearchAccess()
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DatabaseName);
            productsCollection = db.GetCollection<Product>(inventory);
        }




        private List<string> ExtractKeywords(string input)
        {
            // split by spaces and remove duplicates
            return input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList();
        }
        // public async Task<List<InventorySearchResponseDTO>> SearchProduct(string input)
        // {
        //
        //     List<string> keywords = ExtractKeywords(input);
        //     List<Product> inventoryData = await productsCollection.Find(new BsonDocument()).ToListAsync();
        //     Console.WriteLine(inventoryData[0].Category);
        //
        //     // Case-insensitive search in Inventory table
        //     var results = inventoryData
        //         .Where(item => item.Quantity > 0 &&
        //                        item.items.Any(seller => seller.quantity > 0) &&
        //                        (keywords.Any(keyword =>
        //                             item.ProductName.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
        //                         keywords.Any(keyword =>
        //                             item.items.Any(seller => seller.Tags.Contains(keyword, StringComparer.OrdinalIgnoreCase)))))
        //         .SelectMany(item => item.items.Where(seller => seller.quantity > 0), (item, seller) => new InventorySearchResponseDTO
        //         {
        //             id = item.id,
        //             name = item.ProductName,
        //             category = item.Category,
        //             description = seller.Descriptions,
        //             imagesURL = seller.ImageUrl,
        //             Price = seller.quantity > 0 ? seller.Price : int.MaxValue,
        //             SellerId = seller.SellerId
        //         })
        //         .OrderBy(item => item.Price) // Sort  by price
        //             .ToList();
        //
        //     Console.WriteLine(results);
        //
        //     return results;
        // }
        
        public async Task<List<InventorySearchResponseDTO>> SearchProduct(string input)
{
    List<string> keywords = ExtractKeywords(input);
    List<Product> inventoryData = await productsCollection.Find(new BsonDocument()).ToListAsync();
    Console.WriteLine(inventoryData[0].Category);
    // Separate products that match ProductName exactly
    var exactMatchResults = inventoryData
        .Where(item => item.Quantity > 0 &&
                       item.items.Any(seller => seller.quantity > 0) &&
                       keywords.Any(keyword =>
                            item.ProductName.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
        .SelectMany(item => item.items.Where(seller => seller.quantity > 0), (item, seller) => new InventorySearchResponseDTO
        {
            id = item.id,
            name = item.ProductName,
            category = item.Category,
            description = seller.Descriptions,
            imagesURL = seller.ImageUrl,
            Price = seller.quantity > 0 ? seller.Price : int.MaxValue,
            SellerId = seller.SellerId
        });
    // Products that match keywords in seller's tags
    var tagMatchResults = inventoryData
        .Where(item => item.Quantity > 0 &&
                       item.items.Any(seller => seller.quantity > 0) &&
                       keywords.Any(keyword =>
                            item.items.Any(seller => seller.Tags.Contains(keyword, StringComparer.OrdinalIgnoreCase))))
        .SelectMany(item => item.items.Where(seller => seller.quantity > 0), (item, seller) => new InventorySearchResponseDTO
        {
            id = item.id,
            name = item.ProductName,
            category = item.Category,
            description = seller.Descriptions,
            imagesURL = seller.ImageUrl,
            Price = seller.quantity > 0 ? seller.Price : int.MaxValue,
            SellerId = seller.SellerId
        });
    // Concatenate the results, giving priority to exact matches
    var concatenatedResults = exactMatchResults.Concat(tagMatchResults);
    // Group by a unique key (e.g., id) and select the first item from each group
    var results = concatenatedResults
        .GroupBy(item => item.SellerId)
        .Select(group => group.First())
        .ToList();
    Console.WriteLine(results);
    return results;
}
        public async Task<InventoryProductResponseDTO> GetProductByProductNameAndSellerId(string productName, string sellerId)
        {
            List<Product> inventoryData = await productsCollection.Find(new BsonDocument()).ToListAsync();

            // Perform a case-insensitive search in Inventory data
            var result = inventoryData
                .Where(item => item.Quantity > 0 &&
                               item.items.Any(seller => seller.quantity > 0) &&
                               item.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase) &&
                               item.items.Any(seller => seller.SellerId == sellerId))
                .SelectMany(item => item.items.Where(seller => seller.quantity > 0), (item, seller) => new InventoryProductResponseDTO()
                {
                    Id = item.id,
                    ProductName = item.ProductName,
                    Category = item.Category,
                    Description = seller.Descriptions,
                    imagesURL = seller.ImageUrl,
                    Price = seller.Price,
                    SellerId = seller.SellerId
                }).ToList();
               // Return the first match or null if no matches

            
            foreach(var req in result)
            {
                if (req.SellerId == sellerId)
                    return req;
            }
            return null;
        }
}