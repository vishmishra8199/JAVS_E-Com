using System.ComponentModel;

namespace JWT_Token_Example.Inventory.InventoryEditDTO;

public class EditReqDTO
{
    public string ProductName { get; set; }


    public string SellerId { get; set; }



    [DefaultValue(-1)]
    public int quantity { get; set; } = -1;

    [DefaultValue(-1)]
    public int Discount { get; set; } = -1;

    [DefaultValue(-1)]
    public int Price { get; set; } = -1;

    [DefaultValue("unknown")]
    public string ?ImageUrl { get; set; }


    [DefaultValue("unknown")]
    public string? Description { get; set; }
}