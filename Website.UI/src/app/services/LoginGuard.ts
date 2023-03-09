import { Location } from "@angular/common";
import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { AuthenticationService } from "./authentication.service";

@Injectable()

export class LoginGuard implements CanActivate{
    constructor(private _authService: AuthenticationService, private _location: Location){}

    canActivate(): boolean{
        if (this._authService.isLoggedIn()) {
            // User is already logged in, redirect to dashboard
            this._location.back();
            return false;
          }
          return true;
      

        }
}