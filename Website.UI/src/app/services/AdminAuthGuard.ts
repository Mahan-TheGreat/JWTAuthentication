import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { AuthenticationService } from "./authentication.service";

@Injectable()

export class AdminAuthGuard implements CanActivate{
    constructor(private _authService: AuthenticationService, private _router: Router){}

    async canActivate( route: ActivatedRouteSnapshot){
            const isLoggedIn = this._authService.isLoggedIn();
            const requiredRole = route.data['requiredRoles'];
            let userRole = '';
            this._authService.getUserRole()
                                    .subscribe({
                                        next: res=>{
                                            userRole = res;
                                            if(res!= requiredRole){
                                                this._router.navigate(['/dashboard']);
                                                return false;
                                            }else{
                                                return true;
                                            }
                                        }
                                    });
            if(!isLoggedIn){
                this._router.navigate(['/dashboard']);
                return false;
            }
            return true;

        }
}