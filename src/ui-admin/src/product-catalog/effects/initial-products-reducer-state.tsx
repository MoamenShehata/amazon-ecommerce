import type { PagedResult } from "../../core/models/paged-result.models";
import type { ProductForListModel } from "../models/product-for-list-model";

export interface ProductsPageState {
  page: PagedResult<ProductForListModel>;
  isLoading: boolean;
  pageNumber: number;
  pageSize: number;
}

export const initialProductsReducerState: ProductsPageState = {
  page: {
    items: [],
    lastSeenValue: null,
    totalCount: 0,
  },
  isLoading: true,
  pageNumber: 1,
  pageSize: 10,
};
