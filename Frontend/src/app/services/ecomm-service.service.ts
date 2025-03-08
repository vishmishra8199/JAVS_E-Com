import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { map } from 'rxjs';
import { productPost } from '../components/header/Product.model';
import { searchPost } from '../components/header/Search.model';
import { Order, ShoppingCart } from '../components/header/Order.module';
import { cartDetails, cartPost } from '../components/header/Cart.module';
import { reviewGet } from '../components/header/Review.module';
import { UserStoreService } from './user-store.service';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class EcommServiceService {

  // searchForm!: FormGroup;
  recieved_product:any[]=[];
  product_rating=0;
  mycart:cartPost=new cartPost();
  myAddress:string="";
  dummy_imgs=[ "img4.jpg","img5.jpg","img6.jpg","img9.jpg","img8.jpg","img7.jpg"];
  Total_cost = 0;
  product_reviews:reviewGet[]=[];

  individual_pdt_details:productPost= new productPost();
  buyerID: any;

  constructor(private http:HttpClient,
    private userStore : UserStoreService,
    private auth : AuthService) {
  
  }
  
  


  onSearch(searchForm:FormGroup){
    console.log(searchForm.value);
    this.recieved_product=[];
    this.http.post< {[key:string] :searchPost}>(
      
      'https://localhost:7221/ProductFetchingProduct/SearchProduct', searchForm.value,
    ).pipe( map(responseData => {
      const postArray:searchPost[] =[];
      for (const key in responseData){
        if(responseData.hasOwnProperty(key)){
          postArray.push({...responseData[key],searchQuery:key});
        }
      }
      return postArray;
    
    })
      )
    
    .subscribe(response=>{
      console.log("Recieved");
      for(const items of response){
        console.log(items);
        
        this.recieved_product.push(items);

        
      }
    });

    

    
  }



  onGetProductDummy(seller_id:string, product_name:string){
    // this.individual_pdt_details=null;
    this.product_reviews=[];
    this.product_rating=0;

    console.log("hi",seller_id);
    const pdt_url = "https://localhost:7221/ProductFetchingProduct/"+product_name+"/"+seller_id;
    
    this.http.post<any>(
      pdt_url,
      {},
   
    )
    .subscribe(response=>{
      console.log("Product Response---");
      console.log(response);
      console.log("inside pdt");
      
      // this.individual_pdt_details = new productPost();
      this.individual_pdt_details.id = response.id;
      this.individual_pdt_details.sellerId = response.sellerId;
      this.individual_pdt_details.productName = response.productName;
      this.individual_pdt_details.description = response.description;
      this.individual_pdt_details.imagesURL = response.imagesURL;
      this.individual_pdt_details.price = response.price;
      this.individual_pdt_details.category = response.category;
      console.log(this.individual_pdt_details);
    });

  
      const review_url = "https://localhost:7221/BuyerReview/"+product_name;  
    this.http.get<any>(
      review_url,
      
    )
    .subscribe(response=>{
      console.log("Review Response---");

      for(const items in response.review){
        let review_list = new reviewGet();
        
          review_list.BuyerId = response.review[items].buyerId,
          review_list.Description = response.review[items].description,
          review_list.Rating = response.review[items].rating,
          review_list.ImageURL = response.review[items].imageURL,
        
          console.log("Review List");
        console.log(review_list);
        this.product_reviews.push(review_list);

      }
      this.product_rating=response.avgRating;
      console.log(this.product_rating);
    });

 
    
    
  }


  updateCart(){
    
    
    console.log("h1111");

    const pdt_url = "https://localhost:7221/BuyerCart/mycart"

    console.log("whjcehj"+this.individual_pdt_details.sellerId)
    var cart={
      "sellerId":this.individual_pdt_details.sellerId,
        "productName":this.individual_pdt_details.productName,
        "buyerId":localStorage.getItem('userid'),
        "quantity":5
    }

    this.http.post<any>(
      pdt_url,
      cart
      
   
    )
    .subscribe(response=>{
      console.log("Cart DB update Response--- no use");
      console.log(response);
    });

    
  }


  openCart(){
    
    // this.userStore.getUserIdFromStore().subscribe(val=>{
    //   let Id = this.auth.getUseridFromToken();
    //   this.buyerID = val || Id
    // });
    this.buyerID = localStorage.getItem('userid');
    console.log("User di :" + this.buyerID);
    const pdt_url = "https://localhost:7221/BuyerCart/id?id="+this.buyerID;
    this.http.get<any>(
      pdt_url,
    )
    .subscribe(response=>{
      console.log("My cart  Response---");

      console.log(response);
      this.mycart.buyerId = response[0].buyerId;
      this.mycart.items = new Array();
      for(const allItems of response[0].items){
        var cartItems = new cartDetails();
        cartItems.image = allItems.image;
        cartItems.price = allItems.price;
        cartItems.productName = allItems.productName;
        cartItems.quantity = allItems.quantity;
        cartItems.sellerId = allItems.sellerId;
        this.mycart.items.push(cartItems);
      }
    });
  }


  placeOrder(totalCost:number, totalQuantity:number, Address:string){
    this.myAddress=Address;
    this.Total_cost = totalCost;
    var my_orders = new ShoppingCart();
    my_orders.buyerId = this.mycart.buyerId;
    my_orders.totalAmount = totalCost;
    my_orders.totalQuantity = totalQuantity;
    my_orders.billingAddressId="str";

    console.log(this.mycart.items);
    my_orders.orders=new Array<Order>();
    for(const allItems of this.mycart.items){
      var eachItem = new Order();
      eachItem.itemquantity = allItems.quantity;
      eachItem.price=allItems.price;
      eachItem.productName=allItems.productName;
      eachItem.sellerId=allItems.sellerId;

      my_orders.orders.push(eachItem);
    }
    console.log("My orders");
    console.log(my_orders);
    const pdt_url = "https://localhost:7221/OrderBuyer";

    this.http.post<any>(
      pdt_url,
      my_orders
    )
    .subscribe(response=>{
      console.log("My order  Response---");
      console.log(response);

    });


  }
// Vendors functions
  onSubmitProduct(){

  }

  onSubmitImage(){

  }
}


// Search post from header
// product post form header