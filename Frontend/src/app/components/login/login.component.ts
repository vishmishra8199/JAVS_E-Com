import { HttpResponse } from '@angular/common/http';
import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Route, Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { CookieService } from 'ngx-cookie-service';
import ValidateForm from 'src/app/helpers/validateform';
import { AuthService } from 'src/app/services/auth.service';
import { ResetPasswordService } from 'src/app/services/reset-password.service';
import { UserStoreService } from 'src/app/services/user-store.service';
import { PasswordStrengthComponent } from '../password-strength/password-strength.component';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

    cookieHeader : string | null = null;
    type: string = "password";
    isText: boolean = false;
    eyeIcon: string = "fa-eye-slash";
    loginForm!: FormGroup;
    passwordIsValid = false;
    public resetPasswordEmail!:string;
    public isValidEmail! : boolean; 

    // @ViewChild('myModal') modal: any;

    constructor(
      private fb: FormBuilder, 
      private auth: AuthService, 
      private router: Router,
      private toast: NgToastService,
      private userStore : UserStoreService,
      private resetService: ResetPasswordService, 
      private elementRef: ElementRef
      ){

    }

    ngOnInit(): void{
      this.loginForm = this.fb.group({
        username: ['',Validators.required],
        password: ['',Validators.required]
      })


    }

    passwordValid(event : any) {
      this.passwordIsValid = event;
    }

    hideShowPassword(){
      this.isText = !this.isText;
      this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
      this.isText ? this.type = "text" : this.type = "password";
    }

    onLogin(){
      if(this.loginForm.valid){
        
        //Send the object to database
        this.auth.login(this.loginForm.value)
        .subscribe({
          next:(res)=>{
            // alert(res.message);
            // this.auth.storeToken(res.token);
            // let tokenPayload = this.auth.decodeToken();
            
            this.userStore.setFullNameForStore(res.body.name);
            this.userStore.setRoleForStore(res.body.role);
            this.userStore.setUserIdFromStore(res.body.guid.toString());

            this.toast.success({detail:"SUCCESS", summary:"Login Success!", duration: 5000});
            this.router.navigate(['dashboard']);
            if(res.body.role == "User"){
              this.loginForm.reset();
              this.router.navigate(['dashboard']);
            }
            else{
              this.loginForm.reset();
              this.router.navigate(['vendor-dashboard']);
            }
          },
          error:(err)=>{
            // alert(err?.error.message)
            this.toast.error({detail:"ERROR", summary:"Something went wrong!", duration: 5000});
          }
        })
      }
      else{
        //throw error using toaster and with required field.
        ValidateForm.validateAllFormFields(this.loginForm);
        alert("Your form is invaild");
      }
    }

    checkValidEmail(event: string){
      const value = event;
      const pattern = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,3}$/;
      this.isValidEmail = pattern.test(value);
      return this.isValidEmail;
    }

    confirmToReset(){
      if(this.checkValidEmail(this.resetPasswordEmail)){
        
        //api call to be done

        this.resetService.sendResetPasswordLink(this.resetPasswordEmail)
        .subscribe({
          next:(res)=>{
            this.toast.success({
              detail: 'Success',
              summary: 'Password reset was Success!',
              duration: 5000,
            });
            this.resetPasswordEmail ="";
            const buttonRef = document.getElementById("closeBtn");
            buttonRef?.click();
          },
          error:(err)=>{
            this.toast.error({
              detail: 'ERROR',
              summary: 'Something went wrong! password reset',
              duration: 5000,
            });
          }
        })
      }
    }
    @ViewChild('myTest') modal: any;

  openModal() {
    // this.elementRef.nativeElement.style.display = 'block';
    // this.modal.openModal();

    // const dialogRef = this.dialog.open(this.myModal);

    // dialogRef.afterClosed().subscribe(result => {
    //   console.log('The dialog was closed');
    //   this.animal = result;
    // });

  }

  closeModal():void{

  }


}
