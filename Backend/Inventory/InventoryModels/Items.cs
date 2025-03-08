namespace JWT_Token_Example.Inventory.InventoryModels;

public class Items
{
    public string ProductName { get; set; }

        
    public string SellerId { get; set; }

    public string Category { get; set; }

    public List<string> Tags { get; set; }

    public string Descriptions { get; set; }

    public int quantity { get; set; }

    public int Discount { get; set; }

    public int Price { get; set; }

    public string ImageUrl { get; set; }

    public DateTime DateUploaded { get; set; }



    public string Status { get; set; }

}