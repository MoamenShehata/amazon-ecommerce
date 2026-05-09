import { inject, Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  RouterStateSnapshot,
} from '@angular/router';
import { AuthService } from '../../authentication/services/authentication.service';

// @Injectable({
//   providedIn: 'root',
// })
// class AuthGuard {

//   constructor() {}
// }

export const authGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot,
) => {
  const authService = inject(AuthService);
  if (!authService.isAuthenticated) {
    authService.initiateCodeFlow('/cart/checkout');
    return false;
  }

  return true;
};
