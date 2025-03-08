namespace JWT_Token_Example.Carts;

public class EditCartDTO
{
    public string BuyerId { get; set; }

    public EditCartItemsDTO item { get; set; } 
}

public class EditCartItemsDTO
{
    public string SellerId { get; set; }

   
    public int Quantity { get; set; }

   
    public string ProductName { get; set; }


}

