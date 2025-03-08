import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import ValidateForm from 'src/app/helpers/validateform';
import { AuthService } from 'src/app/services/auth.service';


@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {

  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  signUpForm!: FormGroup;
  passwordIsValid = false;

  constructor(
    private fb: FormBuilder, 
    private auth: AuthService, 
    private router: Router,
    private toast: NgToastService){

  }

  ngOnInit(): void{
   this.signUpForm = this.fb.group({
      firstName : ['',Validators.required],
      lastName: ['',Validators.required],
      email : ['',Validators.required],
      userName : ['',Validators.required],
      password : ['',Validators.required],
      mobileNumber : ['',Validators.required]
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

  onSignup(){
    if(this.signUpForm.valid){

      //push data into database
      console.log(this.signUpForm.value);
      this.auth.signUp(this.signUpForm.value)
      .subscribe({
        next:(res)=>{
          console.log("res",res)
          // alert(res.message);
          this.toast.success({detail:"SUCCESS", summary:res.message, duration: 5000});
          this.signUpForm.reset();
          this.router.navigate(['login']);
        },
        error:(err)=>{
          // console.log('Hello world' + err);
          alert(err.message);
          // this.toast.error({detail:"ERROR", summary:"Something wrong", duration: 5000});
        }
      })
    }
    else{

      ValidateForm.validateAllFormFields(this.signUpForm);
        //logic for throwing errors

    }
  }


}
