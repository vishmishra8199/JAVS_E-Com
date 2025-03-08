using MongoDB.Bson;
using MongoDB.Driver;

namespace JWT_Token_Example.Reviews.ReviewModels;

public class ReviewDataAccess
{
    private const string ConnectionString = "mongodb+srv://vishalmishra:Kunal8199@cluster0.hqijrs7.mongodb.net/?retryWrites=true&w=majority";
        // add connection string here
        private const string DatabaseName = "SUJITH_DB";
        // add database name here
        private const string ReviewsDB = "reviews";


        private readonly IMongoCollection<Review> reviewsCollection;


        public ReviewDataAccess()
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DatabaseName);
            reviewsCollection = db.GetCollection<Review>(ReviewsDB);
        }


        public async Task<Review> AddReview(AddReviewDTO obj)
        {
            var review = new Review()
            {
                rating = obj.rating,
                BuyerId = obj.BuyerId,
                Description = obj.Description,
                ProductName = obj.ProductName,
                ImageURL =obj.ImageURL
            };

            await reviewsCollection.InsertOneAsync(review);
            return review;
          

        }


        public async Task<List<Review>> GetAllReviews()
        {
            var req= await reviewsCollection.Find(new BsonDocument()).ToListAsync();
            return req;
        }

        public async Task<ProductReview> GetReviewForProduct(string product)
        {
            var filter = Builders<Review>.Filter.Eq("ProductName", product);
            var req = await reviewsCollection.Find(filter).ToListAsync();

            var result = new List<ProductReviewDTO>();

            float avg = 0;
            int i = 0;
      
            foreach(var rev in req)
            {
                var prodRevDto = new ProductReviewDTO()
                {
                    BuyerId = rev.BuyerId,
                    Description = rev.Description,
                    ImageURL = rev.ImageURL,
                    Rating = rev.rating,
                };

                avg += rev.rating;
                i++;
                result.Add(prodRevDto);
            }


            avg /= i;

            var res = new ProductReview()
            {
                review = result,
                avgRating = avg
            };

            return res;
        }
        public async Task Deletereview(DeleteReviewDTO obj)
        {
           

            var namefilter = Builders<Review>.Filter.Eq("BuyerId", obj.BuyerId);

            var prodfilter = Builders<Review>.Filter.Eq("ProductName", obj.ProductName);

            var combinefilter = Builders<Review>.Filter.And(namefilter, prodfilter);

            await reviewsCollection.DeleteOneAsync(combinefilter);

            
        }

        public async Task<Review> EditReview(EditReviewDTO obj)
        {
            var namefilter = Builders<Review>.Filter.Eq("BuyerId", obj.BuyerId);

            var prodfilter = Builders<Review>.Filter.Eq("ProductName", obj.ProductName);

            var combinefilter = Builders<Review>.Filter.And(namefilter, prodfilter);


            var req = await reviewsCollection.Find((combinefilter)).ToListAsync();

            var revobj = req.First();

            var updated_rating = obj.rating == -1 ? revobj.rating : obj.rating;

            var updated_imageUrl = obj.ImageURL == "unknown" ? revobj.ImageURL : obj.ImageURL;

            var updated_description = obj.Description == "unknown" ? revobj.Description : obj.Description;

            var update_rating = Builders<Review>.Update.Set("rating", updated_rating);

            var update_description = Builders<Review>.Update.Set("Description", updated_description);

            var update_ImageUrl = Builders<Review>.Update.Set("ImageURL", updated_imageUrl);
            
            var combinedUpdate = Builders<Review>.Update.Combine(update_rating, update_description, update_ImageUrl);


            await reviewsCollection.UpdateOneAsync(combinefilter, combinedUpdate);


            var rev = new Review()
            {
                BuyerId = obj.BuyerId,
                Description = obj.Description,
                ImageURL = obj.ImageURL,
                ProductName = obj.ProductName,
                rating = obj.rating
            };

            return rev;
            

        }
}