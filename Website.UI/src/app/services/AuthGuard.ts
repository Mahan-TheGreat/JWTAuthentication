import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { AuthenticationService } from "./authentication.service";

@Injectable()

export class AuthGuard implements CanActivate{
    constructor(private _authService: AuthenticationService, private _router: Router){}

    canActivate(): boolean{
            const isLoggedIn = this._authService.isLoggedIn();
            if(!isLoggedIn){
                this._router.navigate(['/login']);
                return false;
            }else{
                return true;
            }

        }
}