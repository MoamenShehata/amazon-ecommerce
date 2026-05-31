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
  paymentRequestId: string;

  constructor(private cartService: ShoppingCartService) {
    super();
  }

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe((params) => {
      this.paymentRequestId = params.get("id")!;
    });
  }

  checkoutOTP(otp: string) {
    if (!otp) return;


    this.cartService.checkoutUsingOtp(this.paymentRequestId, otp)?.subscribe(
      (orederId) => {
        this.cartService.clearInMemoryCart();
        this.router.navigate(["/my/orders", orederId]);
      },
      (err) => {
        alert(err.error);
      },
    );
  }
}
