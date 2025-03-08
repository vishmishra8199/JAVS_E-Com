using JWT_Token_Example.Inventory.InventoryDeleteDTO;
using JWT_Token_Example.Inventory.InventoryEditDTO;
using JWT_Token_Example.Inventory.InventoryModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace JWT_Token_Example.Inventory.InventoryDataAccess;

public class DataAccess
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

        public DataAccess()
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DatabaseName);
            productsCollection = db.GetCollection<Product>(inventory);
        }


// get all working fine, feature to be only allowed for admin

        public async Task<List<Product>> GetAllP()
        {
          
            var results = await productsCollection.Find(new BsonDocument()).ToListAsync();
            return results;
        }


// get products by id feature for a vendor - DONE

        public async Task<List<Items>> GetAllProducts(string vendorId)
        {
            //var productsCollection = ConnectToMongo<Product>(inventory);
            //var filter = Builders<Product>.Filter.Eq("ProductName", p.ProductName);
            var pr = await productsCollection.Find(new BsonDocument()).ToListAsync();
            var result = new List<Items>();
            foreach(var p in pr)
            {
                foreach(var x in p.items)
                {
                    if(x.SellerId== vendorId)
                    {
                        result.Add(x);
                    }
                }

            }
            //var filter = Builders<Product>.Filter.Eq("items.SellerId", vendorId);
            //var results = await productsCollection.Find(filter).ToListAsync();
            return result;
        }


        // add new items / existing items       

        public async Task AddItem(Items p)
        {



            //int n = 0;

            var filter = Builders<Product>.Filter.Eq("ProductName", p.ProductName);
            var pr = await productsCollection.Find(filter).ToListAsync();
            if (pr.Count() <= 0)
            {
                Product pro = new Product()
                {
                    ProductName = p.ProductName,
                    Category = p.Category,
                    items = new List<Items>(

                        ),

                    Quantity = p.quantity,
                    Status = "Available"


                };

                pro.items.Add(p);

                await productsCollection.InsertOneAsync(pro);
            }
            else
            {
                

                var sellerList = new Items()
                {

                    ProductName=p.ProductName,
                    SellerId = p.SellerId,
                    quantity = p.quantity,
                    //Descriptions = p.Descriptions,
                    //DateUploaded = p.DateUploaded,
                    //Tags = p.Tags,
                    //ImageUrl = p.ImageUrl,
                    //Category = p.Category,
                    //Discount = p.Discount,
                    //Price = p.Price

                };

                var ListSell = new List<Items>();
                //ListSell.Add(sellerList);
                int total_quantity = 0;
                bool flag = false;
                foreach (var products in pr)
                {
                    foreach (var item in products.items)
                    {

                        if (item.SellerId != p.SellerId)
                        {

                           
                            ListSell.Add(item);


                        }
                        else
                        {
                            sellerList.quantity += item.quantity;

                            total_quantity -= item.quantity;
                            sellerList.Descriptions = item.Descriptions;
                            sellerList.DateUploaded = item.DateUploaded;
                            sellerList.Tags = item.Tags;
                            sellerList.ImageUrl = item.ImageUrl;
                            sellerList.Category = item.Category;
                            sellerList.Discount = item.Discount;
                            sellerList.Price = item.Price;
                            sellerList.Status = "available";
                            ListSell.Add(sellerList);
                            flag = true;
                        }


                    }

                    

                    total_quantity += products.Quantity ;

                    total_quantity += sellerList.quantity;

               

                }

                if (flag == false)
                {
                    sellerList.Descriptions = p.Descriptions;
                    sellerList.DateUploaded = DateTime.Now;
                    sellerList.Tags = p.Tags;
                    sellerList.ImageUrl = p.ImageUrl;
                    sellerList.Category = p.Category;
                    sellerList.Discount = p.Discount;
                    sellerList.Price = p.Price;
                    sellerList.Status = "Available";

                    ListSell.Add(sellerList); }

                var update_seller_list = Builders<Product>.Update.Set("items", ListSell);
                //await productsCollection.UpdateOneAsync(filter, update_seller_list);



                var update_quantity = Builders<Product>.Update.Set("Quantity", total_quantity);
                //await productsCollection.UpdateOneAsync(filter, update_quantity);

                var update_status = Builders<Product>.Update.Set("Status", "available");

                var combinedUpdate = Builders<Product>.Update.Combine(update_seller_list, update_quantity, update_status);
                await productsCollection.UpdateOneAsync(filter, combinedUpdate);


            }
        }




        // need to go through edit product as multiple fields should be able to be edited 
        public async Task EditItem(EditReqDTO obj)
        {










            var prodFilter = Builders<Product>.Filter.Eq("ProductName", obj.ProductName);
            var itemFilter = Builders<Product>.Filter.Eq("items.$.SellerId",(obj.SellerId));



            var pr = await productsCollection.Find(prodFilter).ToListAsync();

            if (pr.Count() <= 0)
                return;

            //var product = pr.First();


            //if (obj.isQtyEdited)
            {
    //            var itemUpdate = Builders<Product>.Update
    //                .Set("items.$.quantity", obj.quantity);






    //            // Calculate the total quantity using aggregation framework
    //            var aggregationPipeline = new[]
    //            {
    //    new BsonDocument("$match", new BsonDocument("_id", (obj.ProductName))),
    //    new BsonDocument("$unwind", "$items"),
    //    new BsonDocument("$group", new BsonDocument
    //    {
    //        { "_id",(obj.ProductName) },
    //        { "Quantity", new BsonDocument("$sum", "$items.quantity") }
    //    })
    //};

    //            var totalQuantityUpdate = Builders<Product>.Update
    //                .Set("Quantity", aggregationPipeline);

    //            var statusUpdate = Builders<Product>.Update
    //                .Set("Status", obj.quantity > 0 ? "available" : "not available");

    //            var combinedUpdate = Builders<Product>.Update.Combine(itemUpdate, totalQuantityUpdate, statusUpdate);

    //            await productsCollection.UpdateOneAsync(prodFilter & itemFilter, combinedUpdate);





            }


            var sellerList = new Items()
            {

                ProductName = obj.ProductName,
                SellerId = obj.SellerId,
                //quantity = obj.qua,
                //Descriptions = obj.Descriptions,
                //DateUploaded = obj.DateUploaded,
                //Tags = obj.Tags,
                //ImageUrl = obj.ImageUrl,
                //Category = obj.Category,
                //Discount = obj.Discount,
                //Price = obj.Price,
                //Status = obj.Status

            };
            //Items req= new Items();
           //foreach(Items x in product.items)
           // {
           //     if(x.SellerId==obj.SellerId)
           //     {
           //         req = x;
           //         break;
           //     }    

           // }
           // if (req != sellerList)
           //     return;


            var ListSell = new List<Items>();
            //ListSell.Add(sellerList);
            int total_quantity = 0;
            //bool flag = false;

            
            foreach (var products in pr)
            {

                
                foreach (var item in products.items)
                {

                    if (item.SellerId != obj.SellerId)
                    {


                        ListSell.Add(item);


                    }
                    else
                    {
                        total_quantity -= item.quantity;
                        sellerList.quantity = obj.quantity == -1 ? item.quantity : obj.quantity;

                       
                        sellerList.ImageUrl = obj.ImageUrl == "unknown" ? item.ImageUrl : obj.ImageUrl;
                        sellerList.Category = products.Category;
                        sellerList.Discount = obj.Discount == -1 ? item.Discount : obj.Discount;
                        sellerList.Price = obj.Price == -1 ? item.Price : obj.Price;
                        sellerList.DateUploaded = item.DateUploaded;
                        sellerList.Descriptions = obj.Description == "unknown" ? item.Descriptions : obj.Description;
                        sellerList.Tags = item.Tags;
                        sellerList.Status = obj.quantity <= 0 ? "unavailable" : "available";

                        
                        
                        ListSell.Add(sellerList);

                        
                        //flag = true;
                    }


                }



                total_quantity += products.Quantity;
                total_quantity += sellerList.quantity;



            }

            //if (flag == false)
            //{ ListSell.Add(sellerList); }

            var update_seller_list = Builders<Product>.Update.Set("items", ListSell);
            //await productsCollection.UpdateOneAsync(prodFilter, update_seller_list);

            var update_quantity = Builders<Product>.Update.Set("Quantity", total_quantity);
            //await productsCollection.UpdateOneAsync(prodFilter, update_quantity);

          
            if (total_quantity > 0)
            {
                
                var update_status = Builders<Product>.Update.Set("Status", "available");

                var combinedUpdate = Builders<Product>.Update.Combine(update_seller_list, update_quantity, update_status);
                await productsCollection.UpdateOneAsync(prodFilter, combinedUpdate);
            }

            else
            {
                var update_status = Builders<Product>.Update.Set("Status", "unavailable");

                var combinedUpdate = Builders<Product>.Update.Combine(update_seller_list, update_quantity, update_status);
                await productsCollection.UpdateOneAsync(prodFilter, combinedUpdate);
            }






        }




        public async Task DeleteProduct(DeleteReqDTO obj)
        {


            var filter = Builders<Product>.Filter.Eq("ProductName", obj.ProductName);
            var pr = await productsCollection.Find(filter).ToListAsync();
            int new_prod_quantity = 0;

            if (pr.Count() > 0)
            {

                var product = pr.First();
                 new_prod_quantity = product.Quantity - obj.prev_quantity+obj.new_quantity;

                var update_quantity = Builders<Product>.Update.Set("Quantity", new_prod_quantity);
                await productsCollection.UpdateOneAsync(filter, update_quantity);


                var filter2 = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq("ProductName", obj.ProductName),
                Builders<Product>.Filter.Eq("items.SellerId", obj.SellerId)
                );
                var update = Builders<Product>.Update.Set("items.$.quantity", obj.new_quantity);

                var status_updated = obj.new_quantity <= 0 ? "unavailable" : "available";
                var update_stat = Builders<Product>.Update.Set("items.$.Status", status_updated);
               await productsCollection.UpdateOneAsync(filter2, update);

                await productsCollection.UpdateOneAsync(filter2, update_stat);














                if (new_prod_quantity <= 0)
                {
                    var statusupdate = Builders<Product>.Update.Set("Status", "unavailable");
                    productsCollection.FindOneAndUpdate(filter, statusupdate);

                }


            }
            


        }
}