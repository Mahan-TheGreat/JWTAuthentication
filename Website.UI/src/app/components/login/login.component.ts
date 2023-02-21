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


  
}
