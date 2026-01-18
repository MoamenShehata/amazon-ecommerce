import { Component } from '@angular/core';
import {
  AuthConfig,
  OAuthService,
  OAuthSuccessEvent,
} from 'angular-oauth2-oidc';
import { environment } from '../../../../environments/environment';
import { IdentityControlsComponent } from './identity-controls/identity-controls.component';

@Component({
  selector: 'authentication-landing',
  standalone: true,
  imports: [IdentityControlsComponent],
  templateUrl: './authentication-landing.component.html',
  styleUrl: './authentication-landing.component.scss',
})
export class AuthenticationLandingComponent {
  ngOnInit() {}
}
