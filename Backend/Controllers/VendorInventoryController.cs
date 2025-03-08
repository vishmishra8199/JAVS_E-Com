using JWT_Token_Example.Inventory.InventoryDataAccess;
using JWT_Token_Example.Inventory.InventoryDeleteDTO;
using JWT_Token_Example.Inventory.InventoryEditDTO;
using JWT_Token_Example.Inventory.InventoryModels;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Token_Example.Controllers;
[ApiController]
[Route("[controller]")]
public class VendorInventoryController : ControllerBase
{
    private readonly DataAccess dataAccess;
        
    public VendorInventoryController(DataAccess inventoryServices)
    {
        this.dataAccess = inventoryServices;
    }
    [HttpGet]

    public async Task<List<Product>> GetAll()
    {
        return await dataAccess.GetAllP();
    }

    [HttpGet("{id}")]
    public async Task<List<Items>> GetAll(string id)
    {
        return await dataAccess.GetAllProducts(id);
    }
      
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody]Items SellerINV)
    {
        await dataAccess.AddItem(SellerINV);
           
        return Ok("Done");
    }
    [HttpPut]
    public async Task<IActionResult> EditProduct([FromBody] EditReqDTO SellerINV)
    {
        await dataAccess.EditItem(SellerINV);
        return Ok("Done");
    }

    [HttpDelete]

    //eleteProduct(string vendorId, Items i, int n)

    public async Task<IActionResult> DeleteProduct([FromBody]DeleteReqDTO obj)
    {
        await dataAccess.DeleteProduct(obj);
        return Ok("Done");
    }
}