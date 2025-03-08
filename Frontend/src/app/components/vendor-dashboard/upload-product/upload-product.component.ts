import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { EcommServiceService } from 'src/app/services/ecomm-service.service';

@Component({
  selector: 'app-upload-product',
  templateUrl: './upload-product.component.html',
  styleUrls: ['./upload-product.component.css']
})
export class UploadProductComponent {

  
  productForm!: FormGroup;
  imageForm!: FormGroup;

​
  constructor(
    private my_service:EcommServiceService,
    private auth: AuthService
    ){
​
  }
​
  ngOnInit() {
    this.productForm = new FormGroup({
      'productName':new FormControl(null, Validators.required),
      'sellerId':new FormControl(null, Validators.required),
      'category':new FormControl(null, Validators.required),
      'tags':new FormControl(null, Validators.required),
      'descriptions':new FormControl(null, Validators.required),
      'quantity':new FormControl(null, Validators.required),
      'discount':new FormControl(null, Validators.required),
      'price':new FormControl(null, Validators.required),
    });
    this.imageForm = new FormGroup({
      'image':new FormControl(null, Validators.required),
    });
  };
​
​
  onSubmitProduct(){
    
  }
  onSubmitImage(){
    if(this.imageForm.valid){
      this.auth.login(this.imageForm.value).subscribe()
    }
  }
}
