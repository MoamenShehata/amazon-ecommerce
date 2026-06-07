import { inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../authentication/services/authentication.service';
import { AppRoutes } from './constants/app-routers';
import { ToastrService } from 'ngx-toastr';

export class AppServicesProvider {
  protected router: Router;
  protected activatedRoute: ActivatedRoute;
  protected authService: AuthService;
  protected toastr: ToastrService;

  protected routes = AppRoutes;

  constructor() {
    this.router = inject(Router);
    this.activatedRoute = inject(ActivatedRoute);
    this.authService = inject(AuthService);
    this.toastr = inject(ToastrService);
  }

  protected toastSuccess(message: string, title?: string) {
    try {
      this.toastr.success(message, title);
    } catch (e) {
      // fallback to console if toastr not available
      console.info('Success:', title ? title + ': ' : '', message);
    }
  }

  protected toastError(message: string, title?: string) {
    try {
      this.toastr.error(message, title);
    } catch (e) {
      // fallback to console if toastr not available
      console.error('Error:', title ? title + ': ' : '', message);
    }
  }

  get isAdmin() {
    return this.authService.isAdmin
  }
}
