using JWT_Token_Example.Carts.CartDataAccess;
using JWT_Token_Example.Controllers;
using JWT_Token_Example.Inventory.InventoryDataAccess;
using JWT_Token_Example.Inventory.InventoryEditDTO;
using JWT_Token_Example.Inventory.InventoryModels;
using JWT_Token_Example.Order.OrderModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using JWT_Token_Example.Context;
using JWT_Token_Example.Helpers;
using JWT_Token_Example.Models;
using JWT_Token_Example.Models.DTO;
using JWT_Token_Example.UtilityService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace JWT_Token_Example.Order.OrderDataAccess;

public class OrderDataAccess
{
    private const string ConnectionString = "mongodb+srv://vishalmishra:Kunal8199@cluster0.hqijrs7.mongodb.net/?retryWrites=true&w=majority";
        // add connection string here
        private const string DatabaseName = "SUJITH_DB";
        // add database name here
        private const string OrdersDB = "orders";


        private readonly IMongoCollection<Orders> ordersCollection;

        //private readonly IMongoCollection<Product> productsCollection;


        private DataAccess dataAccess;
        private CartDataAccess cartData;
        
        
        public OrderDataAccess()
        {
           
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DatabaseName);
           ordersCollection = db.GetCollection<Orders>(OrdersDB);
            dataAccess = new DataAccess();
            cartData = new CartDataAccess();
        }


       // for this both products db and orders db
        public async Task<Orders> PlaceOrderBuyer(OrdersDTO obj)
        {



            Guid temp = Guid.NewGuid();
            var orderobj = new Orders()
            {
                id = temp.ToString(),
                orders=new List<OrderItems>(),
                BillingAddressId = obj.BillingAddressId,
                BuyerId = obj.BuyerId
              
            };

            int totalqty = 0, totalamount = 0;
            foreach (var items in obj.orders)
            {
                var fil = Builders<Product>.Filter.Eq("ProductName", items.ProductName);
                var req = await dataAccess.productsCollection.Find(fil).ToListAsync();

                var re = req.First();

                var flag = false;

                foreach (var x in re.items)
                {
                    if (x.quantity < items.itemquantity && x.SellerId == items.SellerId)
                    {
                        flag = true;
                        break;
                    }
                }
                //if (flag)
                //    return null;

                if (!flag)
                {
                    items.Price = await GetPrice(items.ProductName, items.SellerId);
                    for (int i = 0; i < items.itemquantity; i++)
                    {

                        totalqty++;
                        totalamount += items.Price;
                        var item = new OrderItems()
                        {
                            ProductName = items.ProductName,
                            ItemId = Guid.NewGuid(),
                            SellerId = items.SellerId,
                            Price = items.Price,
                            itemquantity = 1,
                            dateOfArrival = DateTime.Today,
                            OrderStatus = "Order Placed"
                        };

                        orderobj.orders.Add(item);
                    }

                    var request = new EditReqDTO()
                    {
                        ProductName = items.ProductName,
                        SellerId = items.SellerId,
                        quantity = items.itemquantity,

                    };

                    await UpdateDBOrderPlaced(request);

        

                }
                
            }
            orderobj.TotalAmount = obj.TotalAmount;
            orderobj.TotalQuantity = obj.TotalQuantity;

         
            await cartData.DeleteCart(obj.BuyerId);
            await ordersCollection.InsertOneAsync(orderobj);
            
            
            
            return orderobj;

        }
        //
        // public async Task SendNotifi(string id, string email, string address)
        // {
        //     
        //     Guid newid;
        //     Guid.TryParse(id, out newid);
        //     var user = await _authContext.Users.FirstOrDefaultAsync(a => a.Id == newid);
        //     var email = user.Email;
        //
        //     string orderNumber = "123";
        //     var address = "";
        //
        //     string from = _configuration["EmailSettings:From"];
        //     var emailModel = new EmailModel(email, "Order Confirmation", UserNotificationBody.UserNotificationMail(orderNumber,address));
        //     _notification.SendEmailAsync(emailModel);
        //     _authContext.Entry(user).State = EntityState.Modified;
        //     await _authContext.SaveChangesAsync();
        //     return Ok(new
        //     {
        //         StatusCode = 200,
        //         Message = "Email Sent!"
        //     });
        // }
        //


        public async Task<int> GetPrice(string pname,string vendorid)
        {
            var prodFilter = Builders<Product>.Filter.Eq("ProductName", pname);

            var pr = await dataAccess.productsCollection.Find(prodFilter).ToListAsync();

            var req = pr.First();

            

            foreach(var items in req.items)
            {
                if (items.SellerId == vendorid)
                    return items.Price;
            }
            return 0;


        }
        public async Task UpdateDBOrderPlaced(EditReqDTO obj)
        {










            var prodFilter = Builders<Product>.Filter.Eq("ProductName", obj.ProductName);
            var itemFilter = Builders<Product>.Filter.Eq("items.$.SellerId", (obj.SellerId));



            var pr = await dataAccess.productsCollection.Find(prodFilter).ToListAsync();

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
                        //total_quantity -= item.quantity;


                        var x = item.quantity-obj.quantity;
                        sellerList.quantity = x;

                        sellerList.ImageUrl = obj.ImageUrl == "unknown" ? item.ImageUrl : obj.ImageUrl;
                        sellerList.Category = products.Category;
                        sellerList.Discount = obj.Discount == -1 ? item.Discount : obj.Discount;
                        sellerList.Price = obj.Price == -1 ? item.Price : obj.Price;
                        sellerList.DateUploaded = item.DateUploaded;
                        sellerList.Descriptions = obj.Description == "unknown" ? item.Descriptions : obj.Description;
                        sellerList.Tags = item.Tags;
                        sellerList.Status = x <= 0 ? "unavailable" : "available";



                        ListSell.Add(sellerList);


                        //flag = true;
                    }


                }



                total_quantity += products.Quantity;
                total_quantity -= obj.quantity;



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
                await dataAccess.productsCollection.UpdateOneAsync(prodFilter, combinedUpdate);
            }

            else
            {
                var update_status = Builders<Product>.Update.Set("Status", "unavailable");

                var combinedUpdate = Builders<Product>.Update.Combine(update_seller_list, update_quantity, update_status);
                await dataAccess.productsCollection.UpdateOneAsync(prodFilter, combinedUpdate);
            }






        }



        // only orders db
        public async Task<List<Orders>> GetAllP()
        {

            var results = await ordersCollection.Find(new BsonDocument()).ToListAsync();
            return results;
        }

    
        //only orders db
        public async Task<List<VendorOrdersDTO>> GetAllOrdersVendor(string vendorId)
        {

            var pr = await ordersCollection.Find(new BsonDocument()).ToListAsync();
            var result = new List<VendorOrdersDTO>();
            foreach (var order in pr)
            {
                foreach (var x in order.orders)
                {
                    if (x.SellerId == vendorId && x.OrderStatus != "Cancelled")
                    {

                       
                            var req = new VendorOrdersDTO()
                            {
                                OrderId = order.id,
                                BillingAddressId = order.BillingAddressId,
                                BuyerId = order.BuyerId,
                                orderitems = new List<VendorOrderItems>()
                            {
                                new VendorOrderItems(){
                                ItemId=x.ItemId,
                                SellerId=x.SellerId,
                                Price=x.Price,
                                itemquantity=x.itemquantity
                                }

                            }
                            };
                            result.Add(req);







                        


                    }
                }

            }

          
            return result;

        }


        // only orders db
        public async Task<List<Orders>> GetOrdersPlacedBuyer(string id)
        {
            var filter = Builders<Orders>.Filter.Eq("BuyerId", id);
            var req = await ordersCollection.Find(filter).ToListAsync();

            return req;

        }

        // both orders and products db
        public async Task<Orders> CancelOrder(CancelOrderDTO obj)
        {

            foreach (var itemid in obj.ItemIds)
            {
                var filter = Builders<Orders>.Filter.And(
                Builders<Orders>.Filter.Eq("id", obj.OrderId),
                Builders<Orders>.Filter.Eq("orders.ItemId", itemid)
                );
                var update = Builders<Orders>.Update.Set("orders.$.OrderStatus", "Cancelled");

                await ordersCollection.UpdateOneAsync(filter, update);


                var filt = Builders<Orders>.Filter.Eq("id", obj.OrderId);
                var pr = await ordersCollection.Find(filt).ToListAsync();
                var order = pr.First();

                string req_productname = "";
                string req_sellerid="";

                foreach(var items in order.orders)
                {
                    if (itemid == items.ItemId)
                    {
                        req_productname = items.ProductName;
                        req_sellerid = items.SellerId;

                        break;
                    }

                }

                var request = new EditReqDTO()
                {

                    ProductName = req_productname,
                    SellerId = req_sellerid,
                    quantity = 1,
                };
                await UpdateDBOrderCancelled(request);



            }




            var filter2 = Builders<Orders>.Filter.Eq("id", obj.OrderId);
            var req = await ordersCollection.Find(filter2).ToListAsync();
            return req.First();
        }

        public async Task UpdateDBOrderCancelled(EditReqDTO obj)
        {










            var prodFilter = Builders<Product>.Filter.Eq("ProductName", obj.ProductName);
            var itemFilter = Builders<Product>.Filter.Eq("items.$.SellerId", (obj.SellerId));



            var pr = await dataAccess.productsCollection.Find(prodFilter).ToListAsync();

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
                        //total_quantity -= item.quantity;
                        var x = item.quantity + 1;
                        sellerList.quantity = x;

                        sellerList.ImageUrl = item.ImageUrl;
                        sellerList.Category = products.Category;
                        sellerList.Discount =  item.Discount ;
                        sellerList.Price = item.Price;
                        sellerList.DateUploaded = item.DateUploaded;
                        sellerList.Descriptions =  item.Descriptions ;
                        sellerList.Tags = item.Tags;
                        sellerList.Status = x <= 0 ? "unavailable" : "available";



                        ListSell.Add(sellerList);


                        //flag = true;
                    }


                }



                total_quantity += products.Quantity;
                total_quantity ++;



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
                await dataAccess.productsCollection.UpdateOneAsync(prodFilter, combinedUpdate);
            }

            else
            {
                var update_status = Builders<Product>.Update.Set("Status", "unavailable");

                var combinedUpdate = Builders<Product>.Update.Combine(update_seller_list, update_quantity, update_status);
                await dataAccess.productsCollection.UpdateOneAsync(prodFilter, combinedUpdate);
            }






        }

}
