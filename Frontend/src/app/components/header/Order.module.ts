export class Order {
    "sellerId": string;
    "price": number;
    "itemquantity": number;
    "productName": string;
    
    
  }
  
export class ShoppingCart {
    "billingAddressId": string;
    "buyerId": string;
    "orders": Array<Order> ;
    "totalAmount": number;
    "totalQuantity": number;
  }