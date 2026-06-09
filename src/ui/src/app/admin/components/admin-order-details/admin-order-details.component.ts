import { Component } from '@angular/core';
import { OrderDetailsSharedComponent } from '../../../core/orders/components/order-details-shared/order-details-shared.component';
import { AppServicesProvider } from '../../../core/services/app-services.provider';
import { CommonModule } from '@angular/common';
import { OrderDetailsDto } from '../../../orders/models/OrderForListDto';

@Component({
  selector: 'app-admin-order-details',
  standalone: true,
  imports: [OrderDetailsSharedComponent, CommonModule],
  templateUrl: './admin-order-details.component.html',
  styleUrl: './admin-order-details.component.css'
})
export class AdminOrderDetailsComponent extends AppServicesProvider {
  orderId: string;

  orderDetails: OrderDetailsDto;

  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      this.orderId = params["id"];
    });
  }

  onOrderLoaded(order: OrderDetailsDto) {
    this.orderDetails = order;
  }
}
