import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
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

  constructor(authenticationService : AuthenticationService){
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
      console.log(this.loginUserForm.value)
    }


  
}
