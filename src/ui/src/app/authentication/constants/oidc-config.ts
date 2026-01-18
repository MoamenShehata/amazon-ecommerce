import { AuthConfig } from 'angular-oauth2-oidc';
import { environment } from '../../../environments/environment';

export const authConfig: AuthConfig = {
  issuer: environment.authenticationBaseUrl,
  responseType: 'code',
  clientId: 'amazon.angular',
  redirectUri: `${window.location.origin}/auth/login`,
  scope: 'openid profile email',

  useSilentRefresh: true,
  silentRefreshRedirectUri: `${window.location.origin}/silent-refresh.html`,
  timeoutFactor: 0.75,

  requireHttps: false,
  postLogoutRedirectUri: window.location.origin,
};
