import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { EcommServiceService } from 'src/app/services/ecomm-service.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {

  searchForm!: FormGroup;
  userName:any;

  ngOnInit() {
    this.searchForm = new FormGroup({
      'searchQuery':new FormControl(null, Validators.required),
    });
    this.userName = localStorage.getItem('fullname');
  };

  constructor(private my_service:EcommServiceService, 
    private router: Router,
    private _auth : AuthService){
    
  }

  onSubmit(){
    this.my_service.onSearch(this.searchForm);
    setTimeout( () => {
      this.router.navigate(['/search-result']);
      
    }, 800 );
  }

  openCart(){
    this.my_service.openCart();
    setTimeout( () => {
      this.router.navigate(['/cart']);
      
    }, 800 );
  }

  logout(){
    this._auth.signOut();
  }

  userprofile(){
    this.router.navigate(['user-profile']);
  }
}
