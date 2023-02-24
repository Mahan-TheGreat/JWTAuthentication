import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { enviroment } from "src/enviroments/enviroment";
import { AuthenticationService } from "./authentication.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor{
    constructor(private _authenticationService: AuthenticationService){

    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const isLoggedIn = this._authenticationService.isLoggedIn();
        const isApiUrl = req.url.startsWith(enviroment.baseApiUrl);
        const token = sessionStorage.getItem('authToken');
        if(isLoggedIn && isApiUrl){
            req = req.clone({
                setHeaders:{
                    Authorization: `Bearer ${token}`
                }
            })
        }
        return next.handle(req);
    }
}