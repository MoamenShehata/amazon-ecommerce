import { PaymentCardDto } from "../customer.services";

export interface CustomerProfile {
  customerId: string;
  addresses: CustomerProfileAddress[];
  paymentCards: PaymentCardDto[];
}

export interface CustomerProfileAddress {
  id?: number;
  country: string;
  city: string;
  street: string;
  buildingNumber: number;
  apartmentNumber: number | null;
}
