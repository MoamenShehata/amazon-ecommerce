export interface CartCreateResultDto {
  cartId: string;
  cartItem: CartItemDto;
}
export interface CartItemModel {
  cartId: string;
  itemId: string;
  productId: string;
  productName: string;
  productImageUrl: string;
  quantity: number;
}

export interface CartItemDto {
  productId: string;
  productName: string;
  productImageUrl: string;
  quantity: number;
  unitPrice?: number;
  isAvailable: boolean;
  subTotal?: number;
}
