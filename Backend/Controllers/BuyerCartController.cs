using JWT_Token_Example.Carts;
using JWT_Token_Example.Carts.CartDataAccess;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Token_Example.Controllers;

[ApiController]
[Route("[controller]")]
public class BuyerCartController
{

    private readonly CartDataAccess dataAccess;
    public BuyerCartController(CartDataAccess cartServices)
    {
        this.dataAccess = cartServices;
    }
    
    [HttpGet("id")]

    public async Task<List<Cart>> GetAll(string id)
    {
        return await dataAccess.GetAllCartItems(id);
    }


    [HttpPost("mycart")]    
    public async Task<Cart> AddProductstoCart(GetProductDto obj)
    {
        var req= await dataAccess.AddtoCart(obj);
        return req;
    }
    
    [HttpDelete]
    public async Task DeleteTotalCart(string BuyerId)
    {
        await dataAccess.DeleteCart(BuyerId);
        
    }

    [HttpPut]

    public async Task<Cart> EditCart(EditCartDTO obj)
    {
        var req = await dataAccess.EditItemsCart(obj);
        return req;
    }

}