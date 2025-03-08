import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { EcommServiceService } from 'src/app/services/ecomm-service.service';
import { cartPost } from '../header/Cart.module';
import { NotificationService } from 'src/app/services/notification.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent {
  mycart:cartPost;
  TotalCost=0;
  TotalQuantity=0;
  mycode:string="";
  Address:string="";
  idw: any ="";
  constructor(private my_service:EcommServiceService, 
    private router: Router,
    private noti : NotificationService) {
    this.mycart= new cartPost();
    
  }
  
  ngOnInit(){
    this.mycart = this.my_service.mycart;
    console.log("Cartttttt");
    console.log(this.mycart);


    for(let cost of this.mycart.items){
      this.TotalCost+=(cost.price*cost.quantity);
      
      this.TotalQuantity+=cost.quantity;
    }
  }

  applyCode(){
    if(this.mycode=='Get10'){
      this.TotalCost-=0.1*this.TotalCost;
    }
    else if(this.mycode=='Get20'){
      this.TotalCost-=0.2*this.TotalCost;
    }
    else if(this.mycode=='Get30'){
      this.TotalCost-=0.3*this.TotalCost;
    }
    else if(this.mycode=='Get40'){
      this.TotalCost-=0.4*this.TotalCost;
    }
    else if(this.mycode=='Get50'){
      this.TotalCost-=0.5*this.TotalCost;
    }
    else{
      this.TotalCost
    }
  }
  orderPlaced(){
    console.log("--------------------------------");
    console.log(this.TotalCost, this.TotalQuantity);
    this.my_service.placeOrder(this.TotalCost,this.TotalQuantity, this.Address);
    this.router.navigate(['/order']);
    this.idw = localStorage.getItem('userid');
    this.noti.sendEmailToUser(this.idw);
    
  }

}
