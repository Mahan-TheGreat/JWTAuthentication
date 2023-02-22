import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RegisterUser } from 'src/app/interface/registerUser.interface';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  private _authenticationService!: AuthenticationService;

  errorFirstName = false;
  errorLastName = false;
  errorEmail = false;
  errorUsername = false;
  errorPassword = false;
  errorConfirmPassword = false;
  errorMatchPassword = false;
  errorLengthPassword = false;

  registerUserForm = new FormGroup({
    firstName: new FormControl('',[Validators.required]),
    lastName: new FormControl('',[Validators.required]),
    username: new FormControl('',[Validators.required]),
    email: new FormControl('',[Validators.required,Validators.email]),
    password: new FormControl('',[Validators.required, Validators.minLength(6)]),
    confirmPassword: new FormControl('',[Validators.required,Validators.minLength(6)])
  })


  get FirstName(){
    return this.registerUserForm.get('firstName');
  }
  get LastName(){
    return this.registerUserForm.get('lastName');
  }
  get Email(){
    return this.registerUserForm.get('email');
  }
  get UserName(){
    return this.registerUserForm.get('username');
  }
  get Password(){
    return this.registerUserForm.get('password');
  }
  get ConfirmPassword(){
    return this.registerUserForm.get('confirmPassword');
  }
 
  constructor(authenticationService: AuthenticationService){
    this._authenticationService= authenticationService;
  }

  private checkNull(){
    var isNull = false;
    if(this.FirstName?.value?.trim()==''){
      isNull = true;
      this.errorFirstName = true;
    }
    if(this.LastName?.value?.trim()==''){
      isNull = true;
      this.errorLastName = true;
    }
    if(this.UserName?.value?.trim()==''){
      isNull = true;
      this.errorUsername = true;
    }
    if(this.Email?.value?.trim()==''){
      isNull = true;
      this.errorEmail = true;
    }
    if(this.Password?.value?.trim()==''){
      isNull = true;
      this.errorPassword = true;
    } 
    if(this.ConfirmPassword?.value?.trim()==''){
      isNull = true;
      this.errorConfirmPassword = true;
    }
    return isNull;
  }

  matchPassword(){
    if(this.Password?.value?.trim()==this.ConfirmPassword?.value?.trim()){
      return false;
    }else{
      this.errorMatchPassword = true;
      return true;
    }
  }

  checkPasswordLength(){
    if(this.Password!.value!.trim().length < 6){
      this.errorLengthPassword = true;
      return true;
    }else{
      return false;
    }
  }

  registerUser(){
    if(this.checkNull() || this.matchPassword() || this.checkPasswordLength()){
      return;
    }
    let user: RegisterUser = {
      firstName: this.registerUserForm.value.firstName!,
      lastName: this.registerUserForm.value.lastName!,
      email: this.registerUserForm.value.email!,
      username: this.registerUserForm.value.username!,
      password: this.registerUserForm.value.password!,
      confirmPassword: this.registerUserForm.value.confirmPassword!
    }
    this._authenticationService.registerUser(user)
        .subscribe({
          next: res=> {
            alert("User registered successfully.");
          },
          error: err=>{
            alert("Something went wrong. Please Try again.");
          }
        })
  }

}
