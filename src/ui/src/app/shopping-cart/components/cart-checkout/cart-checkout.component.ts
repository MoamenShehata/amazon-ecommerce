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

@Component({
  selector: "cart-checkout",
  standalone: true,
  imports: [ShoppingCartComponent, NgFor],
  templateUrl: "./cart-checkout.component.html",
  styleUrl: "./cart-checkout.component.css",
})
export class CartCheckoutComponent extends AppServicesProvider {
  countries: CountryLookup[] = [];

  constructor(
    private customerService: CustomerService,
    private countriesService: CountriesService,
  ) {
    super();
  }

  ngOnInit() {
    this.customerService.getMyProfile().subscribe((res) => {
      alert("Success");
    });

    this.countriesService.getCountries().subscribe((countries) => {
      this.countries = countries;
    });
  }

  cities: CityLookup[] = [];

  onCountrySelected(event: any) {
    const countryId = event.target.value;

    this.cities = this.countries.find((c) => c.id == countryId)?.cities ?? [];
  }
}
