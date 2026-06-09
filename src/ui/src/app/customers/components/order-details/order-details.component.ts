import { Component } from "@angular/core";
import { AppServicesProvider } from "../../../core/services/app-services.provider";
import { OrderDetailsSharedComponent } from "../../../core/orders/components/order-details-shared/order-details-shared.component";
import { NgIf } from "@angular/common";

@Component({
  selector: "app-order-details",
  standalone: true,
  imports: [OrderDetailsSharedComponent, NgIf],
  templateUrl: "./order-details.component.html",
  styleUrl: "./order-details.component.css",
})
export class OrderDetailsComponent extends AppServicesProvider {
  orderId: string;
  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      this.orderId = params["id"];
    });
  }

}
