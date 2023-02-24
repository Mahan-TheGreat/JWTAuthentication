import { Component } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-admindashboard',
  templateUrl: './admindashboard.component.html',
  styleUrls: ['./admindashboard.component.css']
})
export class AdmindashboardComponent {

  username: string = '';
  constructor(private _authenticationService: AuthenticationService){
    this.setUserName();
  }

  setUserName(){
    this._authenticationService.getUserName()
      .subscribe({
        next: res=> this.username = res,
        error:err=> {
          alert("Something went wrong. Please check the console.");
          console.log(err);
        }
      })
  }

}
