import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { OAuthModule } from 'angular-oauth2-oidc';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptors, withInterceptorsFromDi } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { InjectJwtInterceptor } from './core/interceptors/inject-jwt-interceptor';
import { BadRequestInterceptor } from './core/interceptors/bad-request-interceptor';
import { ServerErrorInterceptor } from './core/interceptors/server-error-interceptor';
import { loadingInterceptor } from './core/interceptors/spinner-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi(), withInterceptors([loadingInterceptor])),
    provideAnimations(),
    ...(ToastrModule.forRoot().providers || []),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: InjectJwtInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: BadRequestInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ServerErrorInterceptor,
      multi: true
    },
    ...(OAuthModule.forRoot({
      resourceServer: {
        allowedUrls: ['https://api.yourapp.com'],
        sendAccessToken: true,
      },
    }).providers || []),
  ],
};
