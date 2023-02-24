import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AdmindashboardComponent } from "./components/admindashboard/admindashboard.component";
import { DashboardComponent } from "./components/dashboard/dashboard.component";
import { LoginComponent } from "./components/login/login.component";
import { RegisterComponent } from "./components/register/register.component";
import { AdminAuthGuard } from "./services/AdminAuthGuard";
import { AuthGuard } from "./services/AuthGuard";

 const routes: Routes = [
   {path:'', redirectTo:'login', pathMatch:'full'},
   { path: 'login', component: LoginComponent},
   {path:'register', component: RegisterComponent},
   {path:'dashboard', component: DashboardComponent, canActivate:[AuthGuard]},
   {path:'admin', component: AdmindashboardComponent, canActivate:[AdminAuthGuard], data: {requiredRoles: 'Admin'}},
   { path: '**', redirectTo: 'login', pathMatch: "full" }
 ];

 @NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
 })

 export class AppRoutingModule{
    
 }