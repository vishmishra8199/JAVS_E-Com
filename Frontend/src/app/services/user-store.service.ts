import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserStoreService {

  private fullName$ = new BehaviorSubject<string>("");
  private role$ = new BehaviorSubject<string>("");
  private userid$ = new BehaviorSubject<string>("");
  constructor(
  ) {}

    public getRoleFromStore(){
      return this.role$.asObservable();
    }

    public setRoleForStore(role: string){
      localStorage.setItem('role',role);
      this.role$.next(role);
    }

    public getFullNameFromStore(){
      return this.fullName$.asObservable();
    }

    public setFullNameForStore(fullname: string){
      localStorage.setItem('fullname',fullname);
      this.fullName$.next(fullname);
    }

    public getUserIdFromStore(){
      return this.userid$.asObservable();
    }

    public setUserIdFromStore(id: string){
      localStorage.setItem('userid',id);
      this.userid$.next(id);
    }


}
