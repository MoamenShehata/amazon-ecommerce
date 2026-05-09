import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AuthenticatedUser } from '../../../models/authenticated-user.model';
import { AuthService } from '../../../services/authentication.service';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'identity-controls',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './identity-controls.component.html',
})
export class IdentityControlsComponent {
  authenticatedUser: AuthenticatedUser | null;

  constructor(private authService: AuthService) {
    authService.userSubject.subscribe((u) => {
      this.authenticatedUser = u;
    });
  }

  startOAuthLogin() {
    this.authService.initiateCodeFlow();
  }

  navigateToSignUp() {
    window.location.href = `${environment.authenticationBaseUrl}/Account/RegisterCustomer`;
  }

  logout() {
    this.authService.logoutCurrentUser();
  }

  refreshToken() {
    this.authService.refreshToken();
  }
}
