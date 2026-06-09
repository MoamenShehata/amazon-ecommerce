import { Component } from "@angular/core";
import { AppServicesProvider } from "../../../core/services/app-services.provider";
import { ShoppingCartService } from "../../shopping-cart.services";

@Component({
  selector: "app-cach-checkout",
  standalone: true,
  imports: [],
  templateUrl: "./cach-checkout.component.html",
  styleUrl: "./cach-checkout.component.css",
})
export class CachCheckoutComponent extends AppServicesProvider {
  constructor(private cartService: ShoppingCartService) {
    super();
  }

  ngOnInit() {
  }

  checkoutOTP(otp: string) {
    if (!otp) return;


    this.cartService.confirmPayment(otp)?.subscribe(
      (orederId) => {
        this.cartService.clearInMemoryCart();
        this.router.navigate(["/my/orders", orederId]);
      },
      (err) => {
        this.toastError(err.error);
      },
    );
  }
}
