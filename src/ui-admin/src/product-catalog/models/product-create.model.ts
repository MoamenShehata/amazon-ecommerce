export interface ProductCreateRequest {
  categoryId: string;
  name: string;
  inStockCount: number;
  price: number;
  minimumPrice: number;
  maximumPrice: number;
  properties: ProductProperty[];
}

export interface ProductProperty {
  name: string;
  value: string;
}
