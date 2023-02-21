import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { enviroment } from 'src/enviroments/enviroment';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{

  private baseUrl = enviroment.baseApiUrl;
  private _authenticationService!: AuthenticationService;
  constructor(authenticationService : AuthenticationService){
    this._authenticationService = authenticationService;
  }
  
  ngOnInit(): void {
    this.getUsers();
  }

    getUsers(){
      console.log(`${this.baseUrl}User`);
      this._authenticationService.getUsers()
        .subscribe({
          next: res=> console.log(res)
      })
    }
  
}
