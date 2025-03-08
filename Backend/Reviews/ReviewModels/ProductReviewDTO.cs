namespace JWT_Token_Example.Reviews.ReviewModels;

public class ProductReviewDTO
{
    public string BuyerId { get; set; }

    public string Description { get; set; }

    public long Rating { get; set; }

    public string ImageURL { get; set; }
}