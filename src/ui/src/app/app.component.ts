import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './authentication/services/authentication.service';
import { AppServicesProvider } from './core/services/app-services.provider';
import { IdentityControlsComponent } from './authentication/components/authentication-landing/identity-controls/identity-controls.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, IdentityControlsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent extends AppServicesProvider {
  constructor(authService: AuthService) {
    super();

    authService.configure();
  }
}
