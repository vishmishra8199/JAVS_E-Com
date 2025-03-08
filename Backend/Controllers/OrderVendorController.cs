using JWT_Token_Example.Order.OrderDataAccess;
using JWT_Token_Example.Order.OrderModels;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Token_Example.Controllers;
[ApiController]
[Route("[controller]")]
public class OrderVendorController : ControllerBase
{
    private readonly OrderDataAccess dataAccess;
    public OrderVendorController(OrderDataAccess orderServices)
    {
        this.dataAccess = orderServices;
    }

    [HttpGet]

    public async Task<List<Orders>> GetAll()
    {
        return await dataAccess.GetAllP();
    }



     
    [HttpGet("{id}")]
    public async Task<List<VendorOrdersDTO>> GetAll(string id)
    {
        return await dataAccess.GetAllOrdersVendor(id);
    }
}