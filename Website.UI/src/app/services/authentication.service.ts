import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { enviroment } from "src/enviroments/enviroment";
import { LoginUser } from "../interface/loginUser.interface";
import { RegisterUser } from "../interface/registerUser.interface";

@Injectable({
    providedIn:'root'
})

export class AuthenticationService{

    private baseUrl = enviroment.baseApiUrl;

    private http!: HttpClient;
    constructor(Http: HttpClient){
        this.http = Http;
    }

    registerUser(user: RegisterUser){
       return this.http.post(`${this.baseUrl}User/register`,user);
    }

    loginUser(user: LoginUser){
        return this.http.post(`${this.baseUrl}User/login`,user);
    }
}