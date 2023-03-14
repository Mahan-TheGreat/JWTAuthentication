import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit{

  isLoggedIn = sessionStorage.getItem('authToken') ? true : false;
  items: MenuItem[] = [];
  constructor(private _authenticationService: AuthenticationService, private router:Router){
    console.log(this.isLoggedIn);
    this._authenticationService.loginEvent.subscribe((res) => {
        this.isLoggedIn = true;
        this.updateNavItems();
      })
  }

    ngOnInit() {
        this.setNavItems();
    }

    setNavItems(){
      this.items = [
            {label: 'Dashboard', icon: 'pi pi-user', routerLink:['/dashboard'],visible:true},
            {label: 'Admin', icon: 'pi pi-user', routerLink:['/admin'],visible:true},  
            {label: 'Log Out', icon: 'pi pi-user',visible:this.isLoggedIn,routerLink:['***'], disabled:!this.isLoggedIn,
              routerLinkActiveOptions: {exact:true},
              command: (event) => {
                this.logOut();
            }}
      ];
    }

    updateNavItems() {
      this.items.forEach((item) => {
        if (item.label == 'Log Out') {
          item.visible = this.isLoggedIn;
          item.disabled = !this.isLoggedIn;
        }
      });
    }

    logOut(){
      this.isLoggedIn = false;
      this._authenticationService.logOut();
      this.updateNavItems();
    }
}
