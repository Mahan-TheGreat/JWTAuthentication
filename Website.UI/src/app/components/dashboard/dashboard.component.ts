import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {

  constructor(private _router: Router, private _authenticationService: AuthenticationService){
        this.validateUserLogin();
  }
  validateUserLogin(){
    let loginStatus = this._authenticationService.isLoggedIn();
    if(!loginStatus){
      this._router.navigate(['/login']);
    }
  }
}
