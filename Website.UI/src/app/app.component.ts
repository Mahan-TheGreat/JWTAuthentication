import { Component, OnInit } from '@angular/core';
import { BnNgIdleService } from 'bn-ng-idle';
import { AuthenticationService } from './services/authentication.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  constructor(private bnIdle: BnNgIdleService, private _AuthService: AuthenticationService){}
  title = 'Website.UI';

  ngOnInit(): void{
      this.bnIdle.startWatching(10).subscribe((isTimedOut: boolean) => {
        let isLoggedIn = this._AuthService.isLoggedIn();
        if(isTimedOut && isLoggedIn){
          this._AuthService.logOut();
        }
      })  
  }

}

