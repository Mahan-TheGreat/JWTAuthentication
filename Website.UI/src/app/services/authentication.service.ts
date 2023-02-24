import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { enviroment } from "src/enviroments/enviroment";
import { LoginUser } from "../interface/loginUser.interface";
import { RegisterUser } from "../interface/registerUser.interface";

@Injectable({
    providedIn:'root'
})

export class AuthenticationService{

    private baseUrl = enviroment.baseApiUrl;

    private http!: HttpClient;
    constructor(Http: HttpClient, private _router: Router){
        this.http = Http;
    }

    registerUser(user: RegisterUser){
       return this.http.post<any>(`${this.baseUrl}User/register`,user);
    }

    loginUser(user: LoginUser){
        return this.http.post(`${this.baseUrl}User/login`,user,{responseType:'text'});
    }

    getUserName(){
        return this.http.get(`${this.baseUrl}UserClaims/UserName`,{responseType:'text'});

    }

    getUserRole(){
        return this.http.get(`${this.baseUrl}UserClaims/UserRole`,{responseType:'text'});

    }

    isLoggedIn(){
        let isLoggedIn = false;
        let token = sessionStorage.getItem('authToken');
        if(token){
            isLoggedIn = true;
        }else{
            this._router.navigate(['/login'])
        }
        return isLoggedIn;
    }

    isAdmin(){
        let isAdmin = false;
        this.getUserRole().subscribe({
            next: res => {
                if(res == 'Admin'){
                    isAdmin = true;
                }else{
                    this._router.navigate(['/dashboard']);
                }
            },
            error: err => {
                alert("Something wen wrong! Please check the console.");
                this._router.navigate(['/dashboard']);
                console.log(err);
            }
        })
        return isAdmin;
    }
}