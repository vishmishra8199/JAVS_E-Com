using JWT_Token_Example.Order.OrderDataAccess;
using JWT_Token_Example.Order.OrderModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Token_Example.Controllers;
[ApiController]
[Route("[controller]")]
public class OrderBuyerController : ControllerBase
{
    private readonly OrderDataAccess dataAccess;
    public OrderBuyerController(OrderDataAccess orderServices)
    {
        this.dataAccess = orderServices;
    }

    [HttpGet]

    public async Task<List<Orders>> GetAll()
    {
        return await dataAccess.GetAllP();
    }

    [HttpGet("id")]
    public async Task<List<Orders>> GetAllOrders(string id)
    {
        return await dataAccess.GetOrdersPlacedBuyer(id);
    }

    [HttpPost]

    public async Task<IActionResult> PlaceOrder(OrdersDTO obj)
    {
        var req=await dataAccess.PlaceOrderBuyer(obj);
        if (req == null)
            return BadRequest("Please place the order again");

        return Ok("Order Placed");
    }

    [HttpDelete]

    public async Task<Orders> CancelOrders(CancelOrderDTO obj)

    {
        return await dataAccess.CancelOrder(obj);
    }

}