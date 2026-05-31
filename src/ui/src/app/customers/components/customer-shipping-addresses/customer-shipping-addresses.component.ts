import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CustomerProfileAddress } from "../../models/customer-profile.model";
import { CommonModule, NgIf } from "@angular/common";
import { CustomerShippingAddressFormComponent } from "../customer-shipping-address-form/customer-shipping-address-form.component";

@Component({
  selector: "customer-shipping-addresses",
  standalone: true,
  imports: [CommonModule, CustomerShippingAddressFormComponent],
  templateUrl: "./customer-shipping-addresses.component.html",
  styleUrl: "./customer-shipping-addresses.component.css",
})
export class CustomerShippingAddressesComponent {
  @Input() addresses: CustomerProfileAddress[] = [];
  @Input() style: "select" | "list" = "list";

  @Output() onDeliveryAddressSelected = new EventEmitter<number>();


  emitSelectedAddress(event: any) {
    const addressId = parseInt(event.target.value);
    this.onDeliveryAddressSelected.emit(addressId);
  }

  showAddAddressModal = false;

  openAddAddressModal() {
    this.showAddAddressModal = true;
  }

  closeAddAddressModal() {
    this.showAddAddressModal = false;
  }
}
