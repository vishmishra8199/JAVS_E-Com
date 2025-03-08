namespace JWT_Token_Example.Order.OrderModels;

public class CancelOrderDTO
{
    public string BuyerId { get; set; }

    public List<Guid> ItemIds { get; set; }

    public string OrderId { get; set; }
}