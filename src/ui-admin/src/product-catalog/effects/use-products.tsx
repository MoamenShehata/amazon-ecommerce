import { useEffect, useReducer, useState } from "react";
import type { PageRequest } from "../../core/models/page-request.models";
import ProductsReducer from "../reducers/products-reducer";

import {
  initialProductsReducerState,
  type ProductsPageState,
} from "./initial-products-reducer-state";

export default function UseProductsPage(): [
  ProductsPageState,
  React.ActionDispatch<[action: any]>,
] {
  const [pageState, dispatcher] = useReducer(
    ProductsReducer,
    initialProductsReducerState,
  );

  useEffect(() => {
    let pageRequest: PageRequest = {
      pageNumber: pageState.pageNumber,
      pageSize: pageState.pageSize,
      lastSeenValue: pageState.page.lastSeenValue,
    };

    const sub = catalogServices
      .getProductsPage(pageRequest)
      .subscribe((page) => {
        dispatcher({
          type: "loaded",
          page: page,
        });
      });

    return () => {
      sub.unsubscribe();
      console.log("clean up getting product page");
    };
  }, [pageState.pageNumber]);

  return [pageState, dispatcher];
}

import catalogServices from "../services/catalog.services";
