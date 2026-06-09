import { OrderItemDto } from "./OrderItemDto";

export interface OrderForListDto {
  id: string;
  status: string;
  createdAt: Date;
  createdByEmail: string;
}

export interface OrderDetailsDto {
  id: string;
  status: string;
  statusAdditionalInfo: any;
  totalAmount: number;
  createdAt: Date;
  canBeCanceled: boolean;
  items: OrderItemDto[];
  deliveryAddress: any;
  paymentInfo: any;
}
