import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { enviroment } from "src/enviroments/enviroment";
import { User } from "../interface/Users.interface";

@Injectable({
    providedIn:'root'
})

export class AuthenticationService{

    private baseUrl = enviroment.baseApiUrl;

    private http!: HttpClient;
    constructor(Http: HttpClient){
        this.http = Http;
    }
    getUsers():Observable<User[]>{
       return  this.http.get<User[]>(`${this.baseUrl}User`);
    }
}