import { Component } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import {
  CityLookup,
  CountryLookup,
} from "../../../lookups/models/country-lookup.model";
import { CountriesService } from "../../../lookups/services/countries.service";
import { CommonModule } from "@angular/common";
import { CustomerService } from "../../customer.services";
import { AppServicesProvider } from "../../../core/services/app-services.provider";

@Component({
  selector: "customer-shipping-address-form",
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: "./customer-shipping-address-form.component.html",
  styleUrls: ["./customer-shipping-address-form.component.css"],
})
export class CustomerShippingAddressFormComponent extends AppServicesProvider {
  countries: CountryLookup[] = [];
  cities: CityLookup[] = [];

  shippingAddressForm = this.fb.group({
    city: this.fb.group({
      countryId: [null, Validators.required],
      cityId: [null, Validators.required],
      postalCode: ["", Validators.required],
    }),
    house: this.fb.group({
      street: ["", [Validators.required]],
      buildingNumber: [null, [Validators.required, Validators.min(1)]],
      phoneNumber: [null, [Validators.required]],
      apartmentNumber: [null],
    }),
    isDefault: [false],
  });

  constructor(
    private fb: FormBuilder,
    private countriesService: CountriesService,
    private customerService: CustomerService,
  ) {
    super();
  }

  pageNumber = 1;
  lastSeenValue: any;

  ngOnInit() {
    this.countriesService.getCountries(this.pageNumber, this.lastSeenValue).subscribe((page) => {
      this.countries = page.items;
      this.lastSeenValue = page.lastSeenValue;
    });
  }

  onCountrySelected(event: any) {
    const countryId = event.target.value;

    this.cities = this.countries.find((c) => c.id == countryId)?.cities ?? [];
  }

  submit(): void {
    if (!this.shippingAddressForm.valid) {
      this.shippingAddressForm.markAllAsTouched();
      this.toastError("Please fill all required fields correctly.");
      return;
    }

    const model = this.shippingAddressForm.value;
    console.log("CreateShippingAddressRequest:", model);

    this.customerService.addShippingAddress(model).subscribe({
      next: (res) => {
        this.toastSuccess("Shipping address added successfully.");
        this.shippingAddressForm.reset();
        console.log("Add Shipping Address Response:", res);
      },
      error: (err) => {
        console.error("Error adding shipping address:", err);
        this.toastError(
          "An error occurred while adding the shipping address. Please try again.",
        );
      },
      complete: () => console.log("Add Shipping Address Request completed."),
    });
  }
}
