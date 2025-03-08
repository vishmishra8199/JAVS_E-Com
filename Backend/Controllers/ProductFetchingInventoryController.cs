using JWT_Token_Example.Inventory.InventorySearchAccess;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Token_Example.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductFetchingInventoryController : ControllerBase
{
    private readonly SearchAccess searchAccess;

    public ProductFetchingInventoryController(SearchAccess DataAccess)
    {
        this.searchAccess = DataAccess;
    }

    //[HttpGet]
    //public async Task<List<Product>> GetAll()
    //{

    //    return await searchAccess.GetAllP();
    //}


  
    [HttpPost("SearchProduct")]
    public async Task<IActionResult> SearchProduct(InventorySearchDto searchDto)
    {
        Console.WriteLine(searchDto.searchQuery);
        var result = await searchAccess.SearchProduct(searchDto.searchQuery);

        return Ok(result);

    }
    [HttpPost]
    [Route("{productName}/{sellerId}")]
    public async Task<IActionResult> GetProductByProductNameAndSellerId(string productName, string sellerId)
    {

        try
        {
            var product = await searchAccess.GetProductByProductNameAndSellerId(productName, sellerId);
            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return NotFound($"Product with name '{productName}' and seller ID '{sellerId}' not found.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}