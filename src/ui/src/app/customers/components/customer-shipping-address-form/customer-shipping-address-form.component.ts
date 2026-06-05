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
import { NgSelectComponent } from "@ng-select/ng-select";
import { PagedResult } from "../../../core/models/paged-result.models";

@Component({
  selector: "customer-shipping-address-form",
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, NgSelectComponent],
  templateUrl: "./customer-shipping-address-form.component.html",
  styleUrls: ["./customer-shipping-address-form.component.css"],
})
export class CustomerShippingAddressFormComponent extends AppServicesProvider {
  countries: CountryLookup[] = [];
  cities: CityLookup[] = [];

  isLoading = true;

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

  currentPageNumber = 1;
  lastSeenValue: any;
  totalCountryRecords = 0;

  ngOnInit() {
    this.loadCountriesPage(this.currentPageNumber);
  }

  loadCountriesPage(pageNumber: number) {
    if (this.countries.length == this.totalCountryRecords && this.totalCountryRecords > 0)
      return;

    this.countriesService.getCountries(pageNumber, this.lastSeenValue).subscribe((page) => {
      this.isLoading = false;
      this.countries = [...this.countries, ...page.items];
      this.totalCountryRecords = page.totalCount;

      this.currentPageNumber = pageNumber;
      this.lastSeenValue = page.lastSeenValue;
    });
  }

  onCountrySelected(countryId: any) {
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
