namespace JWT_Token_Example.Order.OrderModels;

public class OrdersDTO
{
    public string BillingAddressId { get; set; }

    public string BuyerId { get; set; }

    public List<OrderItemsDTO> orders { get; set; }

    public long TotalAmount { get; set; }

    public long TotalQuantity { get; set; }
}