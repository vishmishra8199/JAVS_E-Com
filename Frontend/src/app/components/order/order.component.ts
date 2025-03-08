import { Component } from '@angular/core';
import { EcommServiceService } from 'src/app/services/ecomm-service.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent {
  myAddress="";
  TotalCost=0;
  constructor(private my_service:EcommServiceService){
    this.myAddress = my_service.myAddress;
    this.TotalCost = my_service.Total_cost;
  }
}
