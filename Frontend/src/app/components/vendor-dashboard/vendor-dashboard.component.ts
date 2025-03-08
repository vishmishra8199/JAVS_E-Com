import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-vendor-dashboard',
  templateUrl: './vendor-dashboard.component.html',
  styleUrls: ['./vendor-dashboard.component.css']
})
export class VendorDashboardComponent {

    upload : boolean = true;
    displayProd : boolean = false; 
    profile : boolean = false;

    
    constructor(
      private auth : AuthService
    ) {
      
    }


    uploadProduct(){
      this.upload = true;
      this.displayProd = false;
      this.profile = false;
    }

    displayProduct(){
      this.displayProd = true;
      this.upload = false;
      this.profile = false;
    }

    vendorProfile(){
      this.profile = true;
      this.displayProd = false;
      this.upload = false;
    }

    logout(){
      this.auth.signOut();
    }
 }
