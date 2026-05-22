export interface CustomerProfile {
  customerId: string;
  addresses: CustomerProfileAddress[];
}

export interface CustomerProfileAddress {
  id?: number;
  country: string;
  city: string;
  street: string;
  buildingNumber: number;
  apartmentNumber: number | null;
}
