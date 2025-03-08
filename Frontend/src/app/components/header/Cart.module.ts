export class cartPost{
    
    
    "buyerId":string;
    "items":Array<cartDetails>;

}

export class cartDetails{
         "sellerId": string;
        "quantity": number;
        "productName": string;
        "price": number;
        "image": string;
}