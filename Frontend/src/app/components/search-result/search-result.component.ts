import { Component } from '@angular/core';
import { searchPost } from '../header/Search.model';
import { EcommServiceService } from 'src/app/services/ecomm-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchResultComponent {

   // @Input() searchResponse!:searchPost[];
   img_lis:string[]=[];
   Product_details:searchPost[]=[];
   fetchedPdt:boolean=false;
 
   constructor(private my_service:EcommServiceService,  private router: Router){
     
     this.Product_details=[];
     
   }
 
   ngOnInit(){
     this.fetchedPdt=true;
     console.log(this.my_service.recieved_product.length);
     for(let items of this.my_service.recieved_product ){
       console.log(items);
       
     }
     this.Product_details=[];
     this.Product_details = this.my_service.recieved_product;
   }
 
   onGetProductDummy(my_product:string, product_name:string){
     console.log("----------");
     console.log(my_product,product_name);
     this.my_service.onGetProductDummy(my_product, product_name);
     console.log("Product is clicked");
     setTimeout( () => {
       this.router.navigate(['/product']);
       
     }, 1000 );
 
   }

}
