import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AuthenticatedUser } from '../../../models/authenticated-user.model';
import { AuthService } from '../../../services/authentication.service';

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

  logout() {
    this.authService.logoutCurrentUser();
  }

  refreshToken() {
    this.authService.refreshToken();
  }
}
