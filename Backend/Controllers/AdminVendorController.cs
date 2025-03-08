// using JWT_Token_Example.VendorProfileModels.VendorProfileModels.VendorProfileDTO;
// using Microsoft.AspNetCore.Http.HttpResults;
// using Microsoft.AspNetCore.Mvc;
//
//
// namespace JWT_Token_Example.Controllers;
//
// [ApiController]
// [Route("[controller]")]
// public class AdminVendorController : ControllerBase
// {
//     private readonly Vendor_Data vendorData;
//
//     public AdminVendorController(Vendor_Data vdata)
//     {
//         vendorData = vdata;
//     }
//
//
//     [HttpGet("getallvendor")]
//     public async Task<IActionResult> GetAllVendor()
//     {
//
//         var vendors = vendorData.GetAllVendors();
//         return Ok(vendors);
//     }
//
//
//     [HttpGet("/getvendorbyid/{id}")]
//     public async Task<IActionResult> GetVendorbyId(string id)
//     {
//         var vendor = vendorData.GetVendorById(id);
//         if (vendor == null)
//             return NotFound();
//
//         return Ok(vendor);
//     }
//
//
//     [HttpPost("createvendor")]
//     public async Task<IActionResult> CreateVendor(VendorProfileDTO vendor)
//     {
//         vendorData.AddVendor(vendor);
//         return CreatedAtAction(nameof(GetVendorbyId), new { id = vendor.UserId }, vendor);
//     }
//
//     [HttpPut("/updatevendor/{id}")]
//     public async Task<IActionResult> UpdateVendor(string id, VendorProfileDTO vendor)
//     {
//         if (id != vendor.UserId)
//         {
//             return BadRequest();
//         }
//         vendorData.UpdateVendor(vendor);
//         return NoContent();
//     }
//
//     [HttpDelete("/deletevendor/{id}")]
//     public async Task<IActionResult> DeleteVendor(string id)
//     {
//         vendorData.DeleteVendor(id);
//         return NoContent();
//     }
// }