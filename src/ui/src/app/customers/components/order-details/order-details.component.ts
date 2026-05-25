import {Component} from "@angular/core";
import {AppServicesProvider} from "../../../core/services/app-services.provider";
import {OrdersService} from "../../../orders/orders.services";
import {CommonModule} from "@angular/common";
import {OrderDetailsDto} from "../../../orders/models/OrderForListDto";

@Component({
  selector: "app-order-details",
  standalone: true,
  imports: [CommonModule],
  templateUrl: "./order-details.component.html",
  styleUrl: "./order-details.component.css",
})
export class OrderDetailsComponent extends AppServicesProvider {
  orderId: string;

  orderDetails: OrderDetailsDto;

  isLoading = false;
  constructor(private ordersService: OrdersService) {
    super();
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      this.orderId = params["id"];

      this.loadOrderDetails();
    });
  }

  loadOrderDetails() {
    this.isLoading = true;
    this.ordersService.getOrderDetails(this.orderId).subscribe({
      next: (details) => {
        this.orderDetails = details;
        this.isLoading = false;
      },
      error: (err) => {
        console.error("Error loading order details:", err);
      },
    });
  }
}
