/* import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AlertService } from "./app-services/alert.service";
import { Router } from "@angular/router";
import { Login } from "../models/login.model";
import { Register } from "../models/register.model";
import { AppConfig } from "../app-config/app.config";

@Injectable()
export class AuthService {
    private serverUrl: string = AppConfig.APP_URL;
    private apiUrl: string = `${this.serverUrl}api/account/`

    constructor(private http: HttpClient, private alertService: AlertService, private router: Router) { 
    
}
    logIn(login: Login) {
    return true; //return null; //this.http.post<AuthToken>(`${this.apiUrl}login`, login)
    }

    signup(newUser: Register) {
        return this.http.post<Register>(`${this.apiUrl}register`, newUser)
    }
} */
/* 
import { Injectable } from "@angular/core";
import { AppConfig } from "../app-config/app.config";
import { HttpClient } from "@angular/common/http";
import { AlertService } from "./app-services/alert.service";
import { Router } from "@angular/router";
import { BehaviorSubject } from "rxjs";
import { User } from "../models/user.model";
import { Register } from "../models/register.model";
import { Login } from "../models/login.model";
import { AuthToken } from "../models/auth-token";

@Injectable()
export class AuthService{
    public user: BehaviorSubject<User> = new BehaviorSubject<User>(null);
    private serverUrl: string = AppConfig.APP_URL;
    private apiUrl: string = `${this.serverUrl}api/account/`;
    private tokenExpirationTimer: any;

    constructor(private http: HttpClient, private alertService: AlertService, private router: Router) {
        
    }

    signup(newUser: Register) {
    return this.http.post<Register>(`${this.apiUrl}register`, newUser)
    }

    logIn(login: Login)
    {
       console.log("pyk" +this.apiUrl)
       return this.http.post<AuthToken>(`${this.apiUrl}login`, login);
       
    }

    public handleAuthentication(email: string, userId: string, token: string, tokenExpirationDate: Date){
        const user = new User(email, userId, token, tokenExpirationDate);
        console.log(user);
        this.user.next(user);
        const expirationDuration = new Date(tokenExpirationDate).getTime() - new Date().getTime();
        this.autoLogOut(expirationDuration);
        localStorage.setItem("userData", JSON.stringify(user));

    }

    autoLogOut(expirationDuration : number) {
        this.tokenExpirationTimer = setTimeout(() => {
            this.logout();
        }, expirationDuration)
    }

    logout() {
        this.user.next(null);
        this.alertService.showInfo("Wylogowano");
        this.router.navigate(['./']);
        localStorage.removeItem('userData');
        if(this.tokenExpirationTimer) {
            clearTimeout(this.tokenExpirationTimer)
        }
        this.tokenExpirationTimer = null;
    }

    
    autoLogIn(){
        const userData: {
            email: string, 
            id: string, 
            _roleId: number,
            _token: string, 
            _tokenExpirationDate: string
        } = JSON.parse(localStorage.getItem('userData'));
        
    }} */