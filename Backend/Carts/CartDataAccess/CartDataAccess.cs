using JWT_Token_Example.Inventory.InventoryDataAccess;
using JWT_Token_Example.Inventory.InventoryModels;
using MongoDB.Driver;

namespace JWT_Token_Example.Carts.CartDataAccess;

public class CartDataAccess
{
    private const string ConnectionString =
            "mongodb+srv://vishalmishra:Kunal8199@cluster0.hqijrs7.mongodb.net/?retryWrites=true&w=majority";

        // add connection string here
        private const string DatabaseName = "SUJITH_DB";

        // add database name here
        private const string cart = "cart";


        private DataAccess dataAccess;
        
        public readonly IMongoCollection<Cart> cartCollection;


        public CartDataAccess()
        {
            
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DatabaseName);
            cartCollection = db.GetCollection<Cart>(cart);
            dataAccess = new DataAccess();
        }


        public async Task<List<Cart>> GetAllCartItems(string BuyerId)
        {
            var filter = Builders<Cart>.Filter.Eq("BuyerId", BuyerId);
            var req = await cartCollection.Find(filter).ToListAsync();
            return req;

        }
        
        public async Task<Cart> AddtoCart(GetProductDto getProductDto)
        {



            var filter = Builders<Product>.Filter.Eq("ProductName", getProductDto.ProductName);
             
            var currentdata = await dataAccess.productsCollection.Find(filter).ToListAsync();
            var req = currentdata.First();
            int currentitemprice = 0;
            string currentitemimage="";
        
            foreach (var x in req.items)
            {
                if (x.SellerId == getProductDto.SellerId)
                {
                    currentitemprice = x.Price;
                    currentitemimage = x.ImageUrl;
                    break;
                }
            }
        
        
            //chaeck if entry with current userid is present or not in Cart Db
            var fil = Builders<Cart>.Filter.Eq("BuyerId", getProductDto.BuyerId);
            var currentItem = await cartCollection.Find(fil).ToListAsync();
            Cart request=new Cart();
            bool flag = false;
            
            
            if (currentItem.Count() > 0)
            {
               var  car = currentItem.First();

               request.Items = car.Items;
               request.BuyerId = car.BuyerId;
               
                flag = true;
            }
            if (flag)
            {
                CartItems cartItem = new CartItems()
                {
                    SellerId = getProductDto.BuyerId,
                    Quantity = getProductDto.quantity,
                    ProductName = getProductDto.ProductName,
                    Price = currentitemprice,
                    Image = currentitemimage
                };

                request.Items.Add(cartItem);

                await cartCollection.DeleteOneAsync(fil);
                await cartCollection.InsertOneAsync(request);
                return request;

            }
            else
            {
                //Create new cart object
               

                CartItems cartItem = new CartItems()
                {
                    SellerId = getProductDto.SellerId,
                    Quantity = getProductDto.quantity,
                    ProductName = getProductDto.ProductName,
                    Price = currentitemprice,
                    Image = currentitemimage
                };
                Cart cartobj = new Cart()
                {
                    BuyerId = getProductDto.BuyerId,
                    Items = new List<CartItems>()
                };
             
                cartobj.Items.Add(cartItem);
            
                await cartCollection.InsertOneAsync(cartobj);

                return cartobj;
            }

            return null;
        }

        public async Task RemoveFromcart(string BuyerId, CartItems item)
        {
            var currentItem = await cartCollection.Find(cart => cart.BuyerId == BuyerId).ToListAsync();
            var req = currentItem.First();
            foreach (var x in req.Items)
            {
                int index;
                if (x == item)
                {
                    req.Items.Remove(x);
                    break;
                }
            }
        }

        public async Task DeleteCart(string BuyerId)
        {
            var filter = Builders<Cart>.Filter.Eq("BuyerId", BuyerId);
            await cartCollection.DeleteOneAsync((filter));
            
        }
       
        public async Task<Cart> EditItemsCart(EditCartDTO obj)
        {
            var filter = Builders<Cart>.Filter.Eq("BuyerId", obj.BuyerId);
            var req = await cartCollection.Find(filter).ToListAsync();
            var overallcart = req.First();

            var finalCart = new Cart();

            finalCart.BuyerId = overallcart.BuyerId;
            finalCart.Items = new List<CartItems>();
            
            foreach (var x in overallcart.Items)
            {
                if (x.ProductName != obj.item.ProductName || x.SellerId != obj.item.SellerId)
                {
                    finalCart.Items.Add(x);
                }
                else if (x.ProductName == obj.item.ProductName && x.SellerId == obj.item.SellerId)
                {

                    CartItems car = new CartItems()
                    {
                        SellerId = x.SellerId,
                        ProductName = x.ProductName,
                        Image = x.Image,
                        Quantity = obj.item.Quantity,
                        Price = x.Price
                    };
                    
                    finalCart.Items.Add(car);
                }
                
            }

            await cartCollection.DeleteOneAsync(filter);

            await cartCollection.InsertOneAsync(finalCart);

            return finalCart;




        }
}