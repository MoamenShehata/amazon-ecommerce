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
  itemIds: number[];
}
