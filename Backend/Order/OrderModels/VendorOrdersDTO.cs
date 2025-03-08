namespace JWT_Token_Example.Order.OrderModels;

public class VendorOrdersDTO
{
    public string OrderId { get; set; }

    public string BillingAddressId { get; set; }

    public string BuyerId { get; set; }

    public List<VendorOrderItems> orderitems { get; set; }

}