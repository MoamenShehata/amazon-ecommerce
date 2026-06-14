import { Component } from "@angular/core";
import { RouterLink, RouterOutlet } from "@angular/router";
import { AuthService } from "./authentication/services/authentication.service";
import { AppServicesProvider } from "./core/services/app-services.provider";
import { IdentityControlsComponent } from "./authentication/components/authentication-landing/identity-controls/identity-controls.component";
import { LoadingSpinnerComponent } from "./core/components/loading-spinner/loading-spinner.component";
import { SignalRService } from "./core/services/signalR-service";
import { from, Observable } from "rxjs";

@Component({
  selector: "app-root",
  standalone: true,
  imports: [RouterOutlet, IdentityControlsComponent, LoadingSpinnerComponent],
  templateUrl: "./app.component.html",
  styleUrl: "./app.component.css",
})
export class AppComponent extends AppServicesProvider {
  constructor(
    authService: AuthService,
    private signalRService: SignalRService,
  ) {
    super();

    const src = from([1, 2, 3]);

    if (authService.isAuthenticated) {
      this.signalRService.startConnection();

      this.signalRService.addReceiveMessageListener(
        'UserMessage',
        (...args: any[]) => {
          debugger;
          this.toastSuccess(args[0]);
          // else this.toastError(args[1]);
        }
      );

    }

    authService.configure();
  }

  itemsCount = 0;
  ngOnInit() { }
}
