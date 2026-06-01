import { Component, EventEmitter, Input, Output } from "@angular/core";
import { AppServicesProvider } from "../../../core/services/app-services.provider";
import { CustomerService } from "../../customer.services";
import { CustomerShippingAddressesComponent } from "../customer-shipping-addresses/customer-shipping-addresses.component";
import { CustomerProfile } from "../../models/customer-profile.model";
import { CommonModule } from "@angular/common";
import { CustomerPaymentCardsComponent } from "../customer-payment-cards/customer-payment-cards.component";

@Component({
  selector: "customer-profile",
  standalone: true,
  imports: [CommonModule, CustomerShippingAddressesComponent, CustomerPaymentCardsComponent],
  templateUrl: "./customer-profile.component.html",
  styleUrl: "./customer-profile.component.css",
})
export class CustomerProfileComponent extends AppServicesProvider {
  myProfile: CustomerProfile;

  @Input() isReadOnly = false;
  @Input() displayDeliveryAddresses = true;
  @Input() displayPaymentCards = true;

  @Output() onDeliveryAddressSelected = new EventEmitter<number>();
  @Output() onPaymentCardSelected = new EventEmitter<number>();

  constructor(private customerService: CustomerService) {
    super();
  }

  ngOnInit() {
    this.customerService.getMyProfile().subscribe((res) => {
      this.myProfile = res;
    });
  }

  emitOnDeliveryAddressSelected(event: any) {
    const addressId = parseInt(event);
    this.onDeliveryAddressSelected.emit(addressId);
  }

  emitOnPaymentCardSelected(event: any) {
    const cardId = parseInt(event);
    this.onPaymentCardSelected.emit(cardId);
  }
}
