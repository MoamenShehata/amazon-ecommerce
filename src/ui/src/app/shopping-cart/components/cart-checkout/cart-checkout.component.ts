import {Component} from "@angular/core";
import {ShoppingCartComponent} from "../shopping-cart/shopping-cart.component";
import {AppServicesProvider} from "../../../core/services/app-services.provider";
import {CustomerService} from "../../../customers/customer.services";
import {CountriesService} from "../../../lookups/services/countries.service";
import {
  CityLookup,
  CountryLookup,
} from "../../../lookups/models/country-lookup.model";
import {NgFor} from "@angular/common";
import {CustomerProfileComponent} from "../../../customers/components/customer-profile/customer-profile.component";

@Component({
  selector: "cart-checkout",
  standalone: true,
  imports: [ShoppingCartComponent, NgFor, CustomerProfileComponent],
  templateUrl: "./cart-checkout.component.html",
  styleUrl: "./cart-checkout.component.css",
})
export class CartCheckoutComponent extends AppServicesProvider {
  constructor(private customerService: CustomerService) {
    super();
  }

  ngOnInit() {}
}
