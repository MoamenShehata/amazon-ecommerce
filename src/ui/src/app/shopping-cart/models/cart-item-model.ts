export interface CartItemModel {
  cartId: string;
  itemId: string;
  productId: string;
  productName: string;
  productImageUrl: string;
  quantity: number;
}

export interface CartProductDto {
  productId: string;
  productName: string;
  productImageUrl: string;
  itemIds: number[];
}
