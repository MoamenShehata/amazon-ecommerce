import {Component} from "@angular/core";
import {ShoppingCartComponent} from "../shopping-cart/shopping-cart.component";
import {AppServicesProvider} from "../../../core/services/app-services.provider";
import {CustomerService} from "../../../customers/customer.services";

@Component({
  selector: "cart-checkout",
  standalone: true,
  imports: [ShoppingCartComponent],
  templateUrl: "./cart-checkout.component.html",
  styleUrl: "./cart-checkout.component.css",
})
export class CartCheckoutComponent extends AppServicesProvider {
  constructor(private customerService: CustomerService) {
    super();
  }

  ngOnInit() {
    this.customerService.getMyProfile().subscribe((res) => {
      alert("Success");
    });
  }
}
