import { HttpClient } from "@angular/common/http";
import { EventEmitter, Injectable } from "@angular/core";
import { Router } from "@angular/router";
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
    loginEvent: EventEmitter<boolean> = new EventEmitter();


    constructor(Http: HttpClient, private _router: Router){
        this.http = Http;
    }

    registerUser(user: RegisterUser){
      return this.http.post<any>(`${this.baseUrl}User/register`, user)
        .subscribe({
          next: res => {
            alert("User registered successfully.");
            this._router.navigate(['/login']);
          },
          error: err => {
            alert("Something went wrong. Please Try again.");
          }
        });
    }

    loginUser(user: LoginUser){
        this.http.post(`${this.baseUrl}User/login`,user,{responseType:'text'})
        .subscribe({
            next: res=> {
              alert("Login successfully.");
              sessionStorage.setItem('authToken', res);
              this.loginEvent.emit(true);
              this._router.navigate(['/dashboard']);
            },
            error: err=>{
              alert("Error! " + err.error);
            }
          });
    }

    logoutuser(id:number){
        return this.http.post(`${this.baseUrl}User/login`, id);
    }

    getUserName(){
        return this.http.get(`${this.baseUrl}UserClaims/UserName`,{responseType:'text'});

    }

    getUserRole(){
        return this.http.get(`${this.baseUrl}UserClaims/UserRole`,{responseType:'text'});

    }
    getUserId(): Observable<number>{
        return this.http.get<number>(`${this.baseUrl}UserClaims/UserId`);

    }

    isLoggedIn(){
        let isLoggedIn = false;
        let token = sessionStorage.getItem('authToken');
        if(token!==null){
            isLoggedIn = true;
        }
        return isLoggedIn;
    }

    isAdmin(){
        let isAdmin = false;
         this.getUserRole().subscribe({
            next: res => {
                   if(res=='Admin'){
                    isAdmin = true;
                   }      
                   return isAdmin;
    
            },
            error: err => {
                alert("Something went wrong! Please check the console.");
                this._router.navigate(['/dashboard']);
                console.log(err);
                
            }
        })
        return isAdmin;

    }

    logOut(){
        let id = 0;
        this.getUserId().subscribe({
            next: res => {
                id = res
                this.logoutuser(res);
                sessionStorage.removeItem("authToken");
                this._router.navigate(['/login']);
            }
        })

    }
}
