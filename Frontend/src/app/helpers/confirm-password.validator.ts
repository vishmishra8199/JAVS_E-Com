import { FormGroup } from "@angular/forms";

export function ConfirmPasswordValidator(controlName: string, matchControlName: string){
    return (formGroup: FormGroup)=>{
        const passwordControl = formGroup.controls[controlName];
        const ConfirmPasswordControl = formGroup.controls[matchControlName];

        if(ConfirmPasswordControl.errors && ConfirmPasswordControl.errors['confirmPasswordValidator']){
            return;
        }

        if(passwordControl.value !== ConfirmPasswordControl.value){
            ConfirmPasswordControl.setErrors({confirmPasswordValidator: true})
        }else{
            ConfirmPasswordControl.setErrors(null);
        }
    }
}