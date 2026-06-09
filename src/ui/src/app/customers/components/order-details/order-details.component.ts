import { Component } from "@angular/core";
import { AppServicesProvider } from "../../../core/services/app-services.provider";
import { OrdersService } from "../../../orders/orders.services";
import { CommonModule } from "@angular/common";
import { OrderDetailsDto } from "../../../orders/models/OrderForListDto";
import { JsonToListComponent } from "../../../core/components/json-to-list/json-to-list.component";
import { OrderDetailsSharedComponent } from "../../../core/orders/components/order-details-shared/order-details-shared.component";

@Component({
  selector: "app-order-details",
  standalone: true,
  imports: [OrderDetailsSharedComponent],
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
