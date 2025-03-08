import { Component } from '@angular/core';

@Component({
  selector: 'app-advertisement',
  templateUrl: './advertisement.component.html',
  styleUrls: ['./advertisement.component.css']
})
export class AdvertisementComponent {
  images = [  
    { img: "../assets/images/img1.jpg" },  
    { img: "../assets/images/img2.jpg" },  
    { img: "../assets/images/img3.jpg" },  
    
  ];  

  slideConfig = {  
    "slidesToShow": 1,  
    "slidesToScroll": 1,  
    "dots": true,  
    "infinite": true  
  };  
}
