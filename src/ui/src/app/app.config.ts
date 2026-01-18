import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { OAuthModule } from 'angular-oauth2-oidc';
import { provideHttpClient } from '@angular/common/http';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(),
    ...(OAuthModule.forRoot({
      resourceServer: {
        allowedUrls: ['https://api.yourapp.com'],
        sendAccessToken: true,
      },
    }).providers || []),
  ],
};
