import Container from "../../core/bootstrap/components/bootstrap-container";
import { HeaderWithButton } from "../../core/components/header-with-button";
import { ProductPreview } from "./product-preview";
import DataPaginator from "../../core/components/data-paginator/data-paginator";
import RenderIf from "../../core/render-if";
import { useNavigate } from "react-router-dom";

import UseProductsPage from "../effects/use-products";

export default function ProductList({}) {
  debugger;
  const [pageState, dispatcher] = UseProductsPage();

  const navigate = useNavigate();

  let productsDiv = (pageState.page.items || []).map((p) => (
    <ProductPreview
      key={p.id}
      product={p}
      onDeleted={() => dispatcher({ type: "delete", productId: p.id })}
    />
  ));

  return (
    <Container classes="p-2">
      <HeaderWithButton
        header="Products"
        displayButton={true}
        onClick={() => navigate("/catalog/products/create")}
      />

      <RenderIf flag={pageState.isLoading}>
        <div className="alert alert-info" role="alert">
          Loading products...
        </div>
      </RenderIf>

      <RenderIf
        flag={
          pageState.page != null &&
          pageState.page.totalCount == 0 &&
          !pageState.isLoading
        }
      >
        <div className="alert alert-warning" role="alert">
          No Data found.
        </div>
      </RenderIf>

      <RenderIf
        flag={
          pageState.page &&
          pageState.page.totalCount > 0 &&
          !pageState.isLoading
        }
      >
        {productsDiv}

        <DataPaginator
          currentPageNumber={pageState.pageNumber}
          totalCount={pageState.page.totalCount}
          pageSize={pageState.pageSize}
          onPageChanged={(pageNumber) =>
            dispatcher({ type: "navigateToPage", pageNumber: pageNumber })
          }
        />
      </RenderIf>
    </Container>
  );
}
