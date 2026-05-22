import {Component, Input} from "@angular/core";
import {AppServicesProvider} from "../../../core/services/app-services.provider";
import {CustomerService} from "../../customer.services";
import {CountriesService} from "../../../lookups/services/countries.service";
import {
  CountryLookup,
  CityLookup,
} from "../../../lookups/models/country-lookup.model";
import {NgFor} from "@angular/common";

@Component({
  selector: "customer-profile",
  standalone: true,
  imports: [NgFor],
  templateUrl: "./customer-profile.component.html",
  styleUrl: "./customer-profile.component.css",
})
export class CustomerProfileComponent extends AppServicesProvider {
  countries: CountryLookup[] = [];
  cities: CityLookup[] = [];

  @Input() isReadOnly = true;

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

  onCountrySelected(event: any) {
    const countryId = event.target.value;

    this.cities = this.countries.find((c) => c.id == countryId)?.cities ?? [];
  }
}
