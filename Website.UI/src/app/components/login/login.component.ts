import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginUser } from 'src/app/interface/loginUser.interface';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent{

  private _authenticationService!: AuthenticationService;

  passwordError = false;
  usernameError = false;
  
  loginUserForm = new FormGroup({
    username: new FormControl('',[Validators.required]),
    password: new FormControl('',[Validators.required])

  })

  get Username(){
    return this.loginUserForm.get('username');
  }

  get Password(){
    return this.loginUserForm.get('password');
  }

  constructor(
     authenticationService : AuthenticationService,
     private router: Router
    ){
    this._authenticationService = authenticationService;
  }
  
  private checkNull():boolean{
    let isNull = false;
    if(this.Username?.value?.trim() == ''){
      isNull = true;
      this.usernameError = true;
    }
    if(this.Password?.value?.trim() ==''){
      isNull = true;
      this.passwordError = true;
    }
    return isNull;
  }
  loginUser(){
    if(this.checkNull()){
      return;
    }
    let user: LoginUser = {
      username: this.loginUserForm.value.username!,
      password: this.loginUserForm.value.password!,
    }
    this._authenticationService.loginUser(user)
        .subscribe({
          next: res=> {
            alert("Login successfully.");
            this.router.navigate(['/dashboard']);
          },
          error: err=>{
            console.log(err);
            alert("Error! " + err.error);
          }
        })
  }

  
}
