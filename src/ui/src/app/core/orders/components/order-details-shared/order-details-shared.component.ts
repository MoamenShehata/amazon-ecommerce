import { Component, EventEmitter, Input, Output } from '@angular/core';
import { OrderDetailsDto } from '../../../../orders/models/OrderForListDto';
import { OrdersService } from '../../../../orders/orders.services';
import { AppServicesProvider } from '../../../services/app-services.provider';
import { JsonToListComponent } from '../../../components/json-to-list/json-to-list.component';
import { CommonModule } from '@angular/common';
import { CustomerService } from '../../../../customers/customer.services';
import { CustomerProfileAddress } from '../../../../customers/models/customer-profile.model';

@Component({
  selector: 'order-details-shared',
  standalone: true,
  imports: [CommonModule, JsonToListComponent],
  templateUrl: './order-details-shared.component.html',
  styleUrl: './order-details-shared.component.css'
})
export class OrderDetailsSharedComponent extends AppServicesProvider {
  @Input() orderId: string;

  @Output() loaded = new EventEmitter<OrderDetailsDto>();

  orderDetails: OrderDetailsDto;

  isLoading = false;
  constructor(
    private ordersService: OrdersService,
    private customerService: CustomerService,

  ) {
    super();
  }

  ngOnInit() {
    this.loadOrderDetails();
  }

  deliverToAddress: CustomerProfileAddress;

  loadOrderDetails() {
    this.isLoading = true;
    this.ordersService.getOrderDetails(this.orderId).subscribe({
      next: (details) => {
        this.loaded.emit(details);
        this.orderDetails = details;
        this.customerService.getMyProfile().subscribe((profile) => {
          this.deliverToAddress = profile.addresses.find(a => a.countryId == this.orderDetails.deliveryAddress.city.countryId && a.cityId == this.orderDetails.deliveryAddress.city.cityId)!;
        })
        this.isLoading = false;
      }
    });
  }

  cancelOrder() {
    this.ordersService.cancelOrder(this.orderId).subscribe({
      next: () => {
        this.loadOrderDetails();
      },
      error: (err) => {
        this.toastError(err.error);
      },
    });
  }
}
