import { Component } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-admindashboard',
  templateUrl: './admindashboard.component.html',
  styleUrls: ['./admindashboard.component.css']
})
export class AdmindashboardComponent {
  constructor(private _authenticationService: AuthenticationService){
      this.checkLogin();
  }

  checkLogin(){
    let loginStatus = this._authenticationService.isLoggedIn();
    let isAdmin = this._authenticationService.isAdmin();      
    }
  
}
