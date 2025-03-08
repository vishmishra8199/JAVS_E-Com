namespace JWT_Token_Example.Reviews.ReviewModels;

public class ProductReview
{
    public List<ProductReviewDTO> review { get; set; }

    public float avgRating { get; set; }
}