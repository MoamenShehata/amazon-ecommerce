import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { AuthenticatedUser } from "../../../models/authenticated-user.model";
import { AuthService } from "../../../services/authentication.service";
import { environment } from "../../../../../environments/environment";
import { OAuthService } from "angular-oauth2-oidc";
import { RouterModule } from "@angular/router";
import { ShoppingCartComponent } from "../../../../shopping-cart/components/shopping-cart/shopping-cart.component";

@Component({
  selector: "identity-controls",
  standalone: true,
  imports: [CommonModule, RouterModule, ShoppingCartComponent],
  templateUrl: "./identity-controls.component.html",
})
export class IdentityControlsComponent {
  authenticatedUser: AuthenticatedUser | null;

  constructor(
    private authService: AuthService,
    private oauthService: OAuthService,
  ) {
    authService.userSubject.subscribe((u) => {
      this.authenticatedUser = u;
    });
  }

  startOAuthLogin() {
    this.authService.initiateCodeFlow();
  }

  navigateToSignUp() {
    const appUrlBase = window.location.origin;

    window.location.href = `${environment.authenticationBaseUrl}/Account/RegisterCustomer?ReturnUrl=${appUrlBase}/auth/signin`;
  }

  logout() {
    this.authService.logoutCurrentUser();
  }

  refreshToken() {
    this.authService.refreshToken();
  }
}
