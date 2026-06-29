import type { PagedResult } from "../../core/models/paged-result.models";
import type { ProductsPageState } from "../effects/initial-products-reducer-state";
import type { ProductForListModel } from "../models/product-for-list-model";

export default function ProductsReducer(
  currentState: ProductsPageState,
  action: any,
) {
  switch (action.type) {
    case "loaded": {
      return {
        page: {
          ...action.page,
          items: [...action.page.items],
        },
        isLoading: false,
        pageNumber: currentState.pageNumber,
        pageSize: currentState.pageSize,
      };
    }

    case "navigateToPage": {
      return {
        ...currentState,
        isLoading: true,
        pageNumber: action.pageNumber,
      };
    }

    case "delete": {
      return {
        ...currentState,
        page: {
          items: [
            ...currentState.page.items.filter((x) => x.id != action.productId),
          ],
          totalCount: currentState.page.totalCount - 1,
          lastSeenValue: currentState.page.lastSeenValue,
        },
      };
    }

    default: {
      throw Error("Unknown action: " + action.type);
    }
  }
}
