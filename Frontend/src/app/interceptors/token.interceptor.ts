// import { Injectable } from '@angular/core';
// import {
//   HttpRequest,
//   HttpHandler,
//   HttpEvent,
//   HttpInterceptor,
//   HttpErrorResponse
// } from '@angular/common/http';
// import { Observable, catchError, throwError } from 'rxjs';
// import { AuthService } from '../services/auth.service';
// import { NgToastService } from 'ng-angular-popup';
// import { Router } from '@angular/router';
// import { CookieService } from 'ngx-cookie-service';

// @Injectable()
// export class TokenInterceptor implements HttpInterceptor {

//   constructor(
//     private auth: AuthService, 
//     private toast: NgToastService,
//     private route: Router,
//     private cookieService : CookieService) {}

//   intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    
//     const myToken = this.auth.getToken();
//     if(myToken){
//       request = request.clone({
//         setHeaders: {Authorization: `Bearer ${myToken}`
//       } // "Bearer "+ mytoken

//       })
//     }

//     return next.handle(request).pipe(
//       catchError((err: any)=>{
//         if(err instanceof HttpErrorResponse){
//           if(err.status === 401){
//             this.toast.warning({detail:"Warning", summary:"Token is expired, Please login again"});
//             this.route.navigate(['login'])
//           }
//         }
//         return throwError(()=> new Error("Some other error occured"))
//       })
//     );
//   }
// }
