import {  HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { catchError, Observable, throwError } from "rxjs";
import { enviroment } from "src/enviroments/enviroment";
import { AuthenticationService } from "./authentication.service";
import jwt_decode from 'jwt-decode';
import { Router } from "@angular/router";

@Injectable()
export class AuthInterceptor implements HttpInterceptor{
    constructor( private _authenticationService: AuthenticationService, private _router: Router ){}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const isLoggedIn = this._authenticationService.isLoggedIn();
        const isApiUrl = req.url.startsWith(enviroment.baseApiUrl);
        const token = sessionStorage.getItem('authToken');
        if(isLoggedIn && isApiUrl && token){
            const decodedToken:any = jwt_decode(token);
            if(decodedToken.exp < Date.now() / 1000 ){
                sessionStorage.removeItem('authToken');
                this._authenticationService.logOut();
                this._router.navigate(['/login']);
               
             }else{
                req = req.clone({
                    setHeaders:{
                        Authorization: `Bearer ${token}` 
                    }})
            }
           
        };
        return next.handle(req);
        
    }
}