import { Component } from '@angular/core';
import {
  AuthConfig,
  OAuthService,
  OAuthSuccessEvent,
} from 'angular-oauth2-oidc';
import { environment } from '../../../../environments/environment';
import { IdentityControlsComponent } from './identity-controls/identity-controls.component';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'authentication-landing',
  standalone: true,
  imports: [IdentityControlsComponent, RouterLink],
  templateUrl: './authentication-landing.component.html',
  styleUrl: './authentication-landing.component.scss',
})
export class AuthenticationLandingComponent {
  ngOnInit() {}
}
