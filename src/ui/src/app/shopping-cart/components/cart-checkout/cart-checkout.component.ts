import { Component } from "@angular/core";
import { ShoppingCartComponent } from "../shopping-cart/shopping-cart.component";
import { AppServicesProvider } from "../../../core/services/app-services.provider";
import { CustomerService } from "../../../customers/customer.services";
import { CountriesService } from "../../../lookups/services/countries.service";
import {
  CityLookup,
  CountryLookup,
} from "../../../lookups/models/country-lookup.model";
import { CommonModule, NgFor } from "@angular/common";
import { CustomerProfileComponent } from "../../../customers/components/customer-profile/customer-profile.component";
import { PaymentMethod } from "../../models/payment-method.model";
import { PaymentsService } from "../../services/payments.service";

@Component({
  selector: "cart-checkout",
  standalone: true,
  imports: [ShoppingCartComponent, CustomerProfileComponent, CommonModule],
  templateUrl: "./cart-checkout.component.html",
  styleUrl: "./cart-checkout.component.css",
})
export class CartCheckoutComponent extends AppServicesProvider {
  constructor(private customerService: CustomerService,
    private paymentsService: PaymentsService) {
    super();
  }

  paymentMethods: PaymentMethod[] = [];

  ngOnInit() {
    this.paymentsService.getPaymentMethods().subscribe((methods) => {
      this.paymentMethods = methods;
    });
  }

  paymentMethod: PaymentMethod;

  deliverToAddress: number | null = null;
  setDeliveryAddress(addressId: number) {
    alert("Selected delivery address id: " + addressId);
    this.deliverToAddress = addressId;
  }

  onPaymentMethodChange(event: any) {
    const paymentMethodId = parseInt(event.target.value);

    // this.paymentsService.createPaymentRequest(paymentMethodId)
    // this.paymentMethod = this.paymentMethods.find(
    //   (m) => m.id === paymentMethodId,
    // )!;
  }

  proceedToPayment() {
    this.router.navigate([this.paymentMethod.actionRoute]);
  }
}
