import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { EcommServiceService } from 'src/app/services/ecomm-service.service';

@Component({
  selector: 'app-display-pdt',
  templateUrl: './display-pdt.component.html',
  styleUrls: ['./display-pdt.component.css']
})
export class DisplayPdtComponent {
  img_link:string[]=[]; 

  constructor(private my_service:EcommServiceService, private router: Router){
    this.img_link = my_service.dummy_imgs;
  }

  ngOnInit() {

      console.log(this.img_link);

  }

  onGetProductDummy(my_product:string, product_name:string){
    this.my_service.onGetProductDummy(my_product, product_name);
    console.log("Product is clicked");
    setTimeout( () => {
      this.router.navigate(['/product']);
      
    }, 2000 );

  }

}
