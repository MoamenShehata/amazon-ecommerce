import { useEffect, useState } from "react";
import Container from "../../core/bootstrap/components/bootstrap-container";
import { HeaderWithButton } from "../../core/components/header-with-button";
import type { PagedResult } from "../../core/models/paged-result.models";
import type { ProductForListModel } from "../models/product-for-list-model";
import { MayBeEmptyList } from "../../core/components/may-be-empty-list";
import { ProductPreview } from "./product-preview";
import type { PageRequest } from "../../core/models/page-request.models";
import DataPaginator from "../../core/components/data-paginator/data-paginator";
import RenderIf from "../../core/render-if";
import { useNavigate } from "react-router-dom";

import catalogServices from "../services/catalog.services";

export default function ProductList({}) {
  const pageSize = 10;

  const [productsPage, setProductsPage] = useState<
    PagedResult<ProductForListModel>
  >({
    items: [],
    lastSeenValue: null,
    totalCount: 0,
  });
  const [lastSeenValue, setLastSeenValue] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [pageNumber, setPageNumber] = useState(1);

  const navigate = useNavigate();

  useEffect(() => {
    let pageRequest: PageRequest = {
      pageNumber: pageNumber,
      pageSize: pageSize,
      lastSeenValue: lastSeenValue,
    };

    catalogServices.getProductsPage(pageRequest).then(
      (page) => {
        setProductsPage(page.data);
        setLastSeenValue(page.data.lastSeenValue);
        setIsLoading(false);
      },
      (err) => {
        alert("Error");
        console.log(err);
      },
    );

    return () => {
      console.log("clean up getting product page");
    };
  }, [pageNumber]);

  let productsHeader = (
    <HeaderWithButton
      header="Products"
      displayButton={true}
      onClick={() => navigate("/catalog/products/create")}
    />
  );

  function onProductDeleted(product: ProductForListModel) {
    setProductsPage({
      totalCount: productsPage!?.totalCount - 1,
      lastSeenValue: productsPage?.lastSeenValue,
      items: productsPage?.items.filter((x) => x.id != product.id)!,
    });
  }

  let productsDiv = (productsPage?.items || []).map((p) => (
    <ProductPreview
      key={p.id}
      product={p}
      onDeleted={() => onProductDeleted(p)}
    />
  ));

  return (
    <Container classes="p-2">
      {productsHeader}

      <RenderIf flag={isLoading}>
        <div className="alert alert-info" role="alert">
          Loading products...
        </div>
      </RenderIf>

      <RenderIf flag={productsPage != null && !isLoading}>
        <div className="row mt-4">
          <MayBeEmptyList list={productsPage.items} component={productsDiv} />
        </div>

        <DataPaginator
          currentPageNumber={pageNumber}
          totalCount={productsPage.totalCount}
          pageSize={pageSize}
          onPageChanged={setPageNumber}
        />
      </RenderIf>
    </Container>
  );
}
