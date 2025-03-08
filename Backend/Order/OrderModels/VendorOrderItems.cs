namespace JWT_Token_Example.Order.OrderModels;

public class VendorOrderItems
{
    public Guid ItemId { get; set; }

    public string SellerId { get; set; }


    public int Price { get; set; }

    public int itemquantity { get; set; }
}