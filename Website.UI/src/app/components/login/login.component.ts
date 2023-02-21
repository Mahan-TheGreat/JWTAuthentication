import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { enviroment } from 'src/enviroments/enviroment';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{

  private _authenticationService!: AuthenticationService;
  
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
  
  ngOnInit(): void {
    this.getUsers();
  }

    getUsers(){
      this._authenticationService.getUsers()
        .subscribe({
          next: res=> console.log(res)
      })
    }

    loginUser(){
      if(this.Username?.value?.trim() == '' || this.Password?.value?.trim() ==''){
        alert("Error! Username or Password can not be empty.");
        return;
      }
      console.log(this.loginUserForm.value)
    }


  
}
