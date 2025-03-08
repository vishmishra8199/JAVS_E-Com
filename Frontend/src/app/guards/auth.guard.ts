// import { Injectable, inject } from '@angular/core';
// import { CanActivateFn, Router } from '@angular/router';
// import { AuthService } from '../services/auth.service';
// import { NgToastService } from 'ng-angular-popup';

// @Injectable({
//   providedIn: 'root'
// })

// class AdminGuard {
//   /**
//    *
//    */
//   constructor(private auth : AuthService, private route : Router, private toast : NgToastService) {
    
//   }
//   canActivate(): boolean{
//     if(this.auth.isLoggedIn()){
//       // this.route.navigate
//       return true;
//     }
//     else{
//       this.toast.error({detail:"ERROR", summary:"Please Login First!"});
//       this.route.navigate(['login']);
//       return false;
//     }
//   }
// }

// export const authGuard: CanActivateFn = (route, state): boolean => {
//   return inject(AdminGuard).canActivate();
// };


