import {OrderItemDto} from "./OrderItemDto";

export interface OrderForListDto {
  id: string;
  status: string;
  createdAt: Date;
}

export interface OrderDetailsDto {
  id: string;
  items: OrderItemDto[];
}
