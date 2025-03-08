using System.ComponentModel;

namespace JWT_Token_Example.Reviews.ReviewModels;

public class EditReviewDTO
{
    public string ProductName { get; set; }

    public string BuyerId { get; set; }


    [DefaultValue("unknown")]
    public string Description { get; set; }


    [DefaultValue(-1)]
    public long rating { get; set; }


    [DefaultValue("unknown")]
    public string ImageURL { get; set; }
}