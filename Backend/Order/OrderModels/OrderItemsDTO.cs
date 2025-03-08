namespace JWT_Token_Example.Order.OrderModels;

public class OrderItemsDTO
{
    public string SellerId { get; set; }

    
    public int Price { get; set; }

    public int itemquantity { get; set; }

    public string ProductName { get; set; }


    public DateTime dateOfArrival { get; set; }


    public string OrderStatus { get; set; }
}