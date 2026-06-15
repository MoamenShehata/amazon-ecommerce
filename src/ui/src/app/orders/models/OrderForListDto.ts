import { OrderItemDto } from "./OrderItemDto";

export interface OrderForListDto {
  id: string;
  status: string;
  createdAt: Date;
  createdByEmail: string;
}

export interface DeliveryAddress {
  city: CityInfo;
  appartment: HouseInfo;
}

export interface CityInfo {
  countryId: number;
  cityId: number;
  postalCode: string;
}

export interface HouseInfo {
  street: string;
  phoneNumber: string;
  buildingNumber: number;
  apartmentNumber: number | null;
}

export interface OrderDetailsDto {
  id: string;
  status: string;
  statusId: number;
  statusAdditionalInfo: any;
  totalAmount: number;
  createdAt: Date;
  canBeCanceled: boolean;
  items: OrderItemDto[];
  deliveryAddress: DeliveryAddress;
  paymentInfo: any;
}
