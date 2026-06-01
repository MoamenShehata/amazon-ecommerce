import { Component } from "@angular/core";
import { ShoppingCartComponent } from "../shopping-cart/shopping-cart.component";
import { AppServicesProvider } from "../../../core/services/app-services.provider";
import { CommonModule } from "@angular/common";
import { CustomerProfileComponent } from "../../../customers/components/customer-profile/customer-profile.component";
import { PaymentMethod } from "../../models/payment-method.model";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { ShoppingCartService } from "../../shopping-cart.services";
import { PaymentsService } from "../../services/payments.service";

@Component({
  selector: "cart-checkout",
  standalone: true,
  imports: [ShoppingCartComponent, CustomerProfileComponent, CommonModule, ReactiveFormsModule],
  templateUrl: "./cart-checkout.component.html",
  styleUrl: "./cart-checkout.component.css",
})
export class CartCheckoutComponent extends AppServicesProvider {

  paymentMethods: PaymentMethod[] = [];

  constructor(
    private cartService: ShoppingCartService,
    private paymentsService: PaymentsService,
    private fb: FormBuilder) {
    super();
    this.initForm();
  }

  ngOnInit() {
    this.paymentsService.getPaymentMethods().subscribe((methods) => {
      this.paymentMethods = methods;
    });
  }

  redirectRoutes: any = {
    0: '/cart/checkout/cash',
    1: '/cart/checkout/card',
  }

  proceedToPayment() {
    if (!this.setupForCheckoutForm.valid) {
      alert("Please select delivery address and payment method");
      return;
    }

    this.cartService.setupForCheckout(this.setupForCheckoutForm.value)
      .subscribe((result) => {
        this.router.navigate([this.redirectRoutes[result]]);
      });
  }

  setupForCheckoutForm: FormGroup;
  initForm() {
    this.setupForCheckoutForm = this.fb.group({
      deliverToAddressId: [null, [Validators.required]],
      paymentMethodId: [null, [Validators.required]],
    });
  }

  setDeliveryAddress(addressId: number) {
    this.setupForCheckoutForm.patchValue({ deliverToAddressId: addressId });
  }
}
