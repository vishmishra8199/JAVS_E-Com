import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ResetPassword } from 'src/app/models/reset-password.model';
import { ReactiveFormsModule } from '@angular/forms';
import { ConfirmPasswordValidator } from 'src/app/helpers/confirm-password.validator';
import { ActivatedRoute, Router } from '@angular/router';
import ValidateForm from 'src/app/helpers/validateform';
import { ResetPasswordService } from 'src/app/services/reset-password.service';
import { NgToastService } from 'ng-angular-popup';

@Component({
  selector: 'app-reset',
  templateUrl: './reset.component.html',
  styleUrls: ['./reset.component.css']
})
export class ResetComponent implements OnInit {
    resetPasswordForm!: FormGroup;
    emailToReset!: string;
    emailToken!: string;
    reserPasswordObj = new ResetPassword();

    constructor(private fb: FormBuilder,
      private activatedRoute : ActivatedRoute,
      private resetService: ResetPasswordService,
      private toast : NgToastService,
      private router: Router
      ) {
      
    }

    ngOnInit(): void {
      this.resetPasswordForm = this.fb.group({
        password: [null, Validators.required],
        confirmPassword:[null,Validators.required]
      },{
        validator: ConfirmPasswordValidator("password","confirmPassword")
      });

      this.activatedRoute.queryParams.subscribe(val =>{
        this.emailToReset = val['email'];
        let uriToken = val['code'];
        this.emailToken = uriToken.replace(/ /g,'+');
      })
    }

    reset(){
      if(this.resetPasswordForm.valid){
        this.reserPasswordObj.email = this.emailToReset;
        this.reserPasswordObj.newPassword = this.resetPasswordForm.value.password;
        this.reserPasswordObj.confirmPassword = this.resetPasswordForm.value.confirmPassword;
        this.reserPasswordObj.emailToken = this.emailToken;
        this.resetService.resetPassword(this.reserPasswordObj)
        .subscribe({
          next:(res)=>{
            this.toast.success({
              detail: 'SUCCESS',
              summary: "Password Reset Successfully",
              duration: 3000,
            });
            this.router.navigate(['/login'])
          },
          error:(err)=>{
            this.toast.error({
              detail: 'ERROR',
              summary: "Something went wrong",
              duration: 3000,
            });
          }
        })

      }
      else{
        ValidateForm.validateAllFormFields(this.resetPasswordForm);
      }
    }
}
