using JWT_Token_Example.Reviews.ReviewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Token_Example.Controllers;

[ApiController]
[Route("[controller]")]
public class BuyerReviewController: ControllerBase
{
    private readonly ReviewDataAccess dataAccess;

    public BuyerReviewController(ReviewDataAccess rev)
    {
        dataAccess = rev;
    }

    [HttpGet]

    public async Task<List<Review>> GetAll()
    {
        return await dataAccess.GetAllReviews();
    }

    [HttpGet("{productname}")]

    public async Task<ProductReview> GetProductReview( string productname)
    {
        return await dataAccess.GetReviewForProduct(productname);
    }

    [HttpPost]

    public async Task<Review> AddReview([FromBody] AddReviewDTO obj)
    {
        return await dataAccess.AddReview(obj);
    }

    [HttpDelete]

    public async Task<IActionResult> DeleteReview([FromBody] DeleteReviewDTO obj)
    {
        await dataAccess.Deletereview(obj);
        return Ok("Done");
    }


    [HttpPut]

    public async Task<Review> EditReview([FromBody] EditReviewDTO obj)
    {
        return await dataAccess.EditReview(obj);
    }

}