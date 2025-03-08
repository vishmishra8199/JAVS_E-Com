import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl:string = "https://localhost:7031/api/User/"
  private imageUrl:string = "https://localhost:7031/api/Image/"
  // private userPayload: any;
  constructor(private http : HttpClient, private router: Router) {
   }

   signUp(userObj: any){
    return this.http.post<any>(`${this.baseUrl}register`,userObj);
   }

   signUpVendor(vendorObj : any){
    return this.http.post<any>(`${this.baseUrl}vendorRegister`,vendorObj);
   }
   

   login(loginObj: any){
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.http.post<any>(`${this.baseUrl}authenticate`,loginObj,{
      headers: header,
      observe: 'response',
      withCredentials: true
    });
   }

   signOut(){
    localStorage.clear();
    this.router.navigate(['login']);
   }

   uploadImage(image : any){
      return this.http.post<any>(`${this.imageUrl}uploadImg`,image);
   }

   getUser(id: any){
    return this.http.post<any>(`${this.baseUrl}getUser`,id,{
      observe: 'response',
      withCredentials: true
    });
   }

  //  storeToken(tokenValue: string){
  //   localStorage.setItem('token',"logedin");
  //  }

  //  getToken(){
  //   return localStorage.getItem('token')
  //  }

  //  isLoggedIn(): boolean{
  //   return !!localStorage.getItem('token')
  //  }

  //  decodeToken(){
  //   const jwtHelper = new JwtHelperService();
  //   const token = this.getToken()!;
  //   return jwtHelper.decodeToken(token);
  //  }

  //  getFullNameFromToken(){
  //   if(this.userPayload)
  //   return this.userPayload.name;
  //  }

  //  getRoleFromToken(){
  //   if(this.userPayload)
  //   return this.userPayload.role;
  //  }

  //  getUseridFromToken(){
  //   if(this.userPayload)
  //   return this.userPayload.guid;
  //  }


}
