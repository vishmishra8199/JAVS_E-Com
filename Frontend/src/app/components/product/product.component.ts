import { Component } from '@angular/core';
import { productPost } from '../header/Product.model';
import { EcommServiceService } from 'src/app/services/ecomm-service.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent {
  individual_pdt:productPost;
  img_loc = "";
  product_review_list:any;
  product_rating:any;

  constructor(private my_service: EcommServiceService){
    this.individual_pdt = new productPost;
    
  }

  ngOnInit(){
    this.img_loc = "../../assets/images/";
    this.product_review_list=[];
    this.product_rating=0;
    this.individual_pdt = this.my_service.individual_pdt_details;
    console.log("Product");
    console.log(this.individual_pdt);
    
  //  this.img_loc = this.img_loc+this.individual_pdt.imagesURL;

  this.img_loc = this.img_loc+this.individual_pdt.imagesURL;
  console.log(this.img_loc);


  this.product_review_list = this.my_service.product_reviews;
  this.product_rating = this.my_service.product_rating;
  
    
  }

  updateCart(){
    alert("Added to cart !");
    this.my_service.updateCart();
  }
}
