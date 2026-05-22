import {Component} from "@angular/core";
import {RouterLink, RouterOutlet} from "@angular/router";
import {AuthService} from "./authentication/services/authentication.service";
import {AppServicesProvider} from "./core/services/app-services.provider";
import {IdentityControlsComponent} from "./authentication/components/authentication-landing/identity-controls/identity-controls.component";
import {ShoppingCartService} from "./shopping-cart/shopping-cart.services";
import {ShoppingCartComponent} from "./shopping-cart/components/shopping-cart/shopping-cart.component";

@Component({
  selector: "app-root",
  standalone: true,
  imports: [RouterOutlet, IdentityControlsComponent, ShoppingCartComponent],
  templateUrl: "./app.component.html",
  styleUrl: "./app.component.css",
})
export class AppComponent extends AppServicesProvider {
  constructor(
    authService: AuthService,
    private shoppingCartService: ShoppingCartService,
  ) {
    super();

    authService.configure();
  }

  itemsCount = 0;
  ngOnInit() {}
}
