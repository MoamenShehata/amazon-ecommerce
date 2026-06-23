export interface ProductForListModel {
  id: string;
  name: string;
  categories: string;
  unitPrice: number;
  imageUrl?: string;
  isAvailable?: boolean;
}