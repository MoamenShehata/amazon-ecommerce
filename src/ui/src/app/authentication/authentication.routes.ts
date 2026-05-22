import {Routes} from "@angular/router";
import {AuthenticationLandingComponent} from "./components/authentication-landing/authentication-landing.component";
import {LoginComponent} from "./login/login.component";
import {SignInComponent} from "./components/sign-in/sign-in.component";

export const authRoutes: Routes = [
  {
    path: "",
    component: AuthenticationLandingComponent,
    children: [],
  },
  {
    path: "login",
    component: LoginComponent,
    children: [],
  },
  {
    path: "signin",
    component: SignInComponent,
    children: [],
  },
];
