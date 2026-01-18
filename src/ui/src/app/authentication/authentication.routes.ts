import { Routes } from '@angular/router';
import { AuthenticationLandingComponent } from './components/authentication-landing/authentication-landing.component';
import { LoginComponent } from './login/login.component';

export const authRoutes: Routes = [
  {
    path: '',
    component: AuthenticationLandingComponent,
    children: [],
  },
  {
    path: 'login',
    component: LoginComponent,
    children: [],
  },
];
