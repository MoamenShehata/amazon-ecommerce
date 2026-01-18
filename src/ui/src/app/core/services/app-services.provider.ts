import { inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../authentication/services/authentication.service';
import { AppRoutes } from './constants/app-routers';

export class AppServicesProvider {
  protected router: Router;
  protected activatedRoute: ActivatedRoute;
  protected authService: AuthService;

  protected routes = AppRoutes;

  constructor() {
    this.router = inject(Router);
    this.activatedRoute = inject(ActivatedRoute);
    this.authService = inject(AuthService);
  }
}
