import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { enviroment } from "src/enviroments/enviroment";

@Injectable({
    providedIn:'root'
})

export class AuthenticationService{

    private baseUrl = enviroment.baseApiUrl;

    private http!: HttpClient;
    constructor(Http: HttpClient){
        this.http = Http;
    }

}