namespace JWT_Token_Example.Inventory.InventoryDeleteDTO;

public class DeleteReqDTO
{
    public string ProductName { get; set; }
    
    public string SellerId { get; set; }
    
    public int prev_quantity { get; set; }
       
    public int new_quantity{ get; set; }
}