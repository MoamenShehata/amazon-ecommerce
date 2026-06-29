import axios from "axios";
import { environment } from "../../environment";
import type { PageRequest } from "../../core/models/page-request.models";
import type { PagedResult } from "../../core/models/paged-result.models";
import type { ProductForListModel } from "../models/product-for-list-model";
import type { CategoryForListModel } from "../models/category-for-list.models";
import type { ProductCreateRequest } from "../models/product-create.model";
import { observable, Observable } from "rxjs";

export class CatalogService {
  private categoriesBaseUrl = `${environment.gatewayBaseUrl}/categories`;
  private productsBaseUrl = `${environment.gatewayBaseUrl}/products`;

  getCategoriesPage(
    pageRequest: PageRequest,
  ): Observable<PagedResult<CategoryForListModel>> {
    return new Observable((observer) => {
      axios
        .get<
          PagedResult<CategoryForListModel>
        >(`${this.categoriesBaseUrl}?pageNumber=${pageRequest.pageNumber}&pageSize=${pageRequest.pageSize}&lastSeenValue=${pageRequest.lastSeenValue}`)
        .then(
          (res) => {
            observer.next(res.data);
            observer.complete();
          },
          (err) => {
            observer.error(err);
          },
        );
    });
  }

  createCategory(categoryRequest: {
    name: string;
    parentCategoryId?: string | null;
  }): Observable<CategoryForListModel> {
    return new Observable((observer) => {
      axios
        .post<CategoryForListModel>(this.categoriesBaseUrl, categoryRequest)
        .then(
          (res) => {
            observer.next(res.data);
            observer.complete();
          },
          (err) => {
            observer.error(err);
          },
        );
    });
  }

  getProductsPage(pageRequest: PageRequest) {
    return new Observable<PagedResult<ProductForListModel>>((observer) => {
      axios
        .get<
          PagedResult<ProductForListModel>
        >(`${this.productsBaseUrl}?pageNumber=${pageRequest.pageNumber}&pageSize=${pageRequest.pageSize}&lastSeenValue=${pageRequest.lastSeenValue}`)
        .then(
          (res) => {
            observer.next(res.data);
            observer.complete();
          },
          (err) => {
            observer.error(err);
          },
        );
    });
  }

  createProduct(productRequest: ProductCreateRequest, image?: File) {
    const formData = new FormData();
    formData.append("categoryId", productRequest.categoryId);
    formData.append("name", productRequest.name);
    formData.append("inStockCount", productRequest.inStockCount.toString());
    formData.append("price", productRequest.price.toString());
    formData.append("minimumPrice", productRequest.minimumPrice.toString());
    formData.append("maximumPrice", productRequest.maximumPrice.toString());

    // Append properties with proper indexing for ASP.NET Core model binding
    productRequest.properties.forEach((prop, index) => {
      formData.append(`properties[${index}].key`, prop.name);
      formData.append(`properties[${index}].value`, prop.value);
    });

    // Append image if provided
    if (image) {
      formData.append("image", image, image.name);
    }

    return new Observable<{ id: string }>((observer) => {
      axios.post<{ id: string }>(this.productsBaseUrl, formData).then(
        (res) => {
          observer.next({ id: res.data.id });
          observer.complete();
        },
        (err) => {
          observer.error(err);
        },
      );
    });
  }

  deleteProduct(id: string) {
    return axios.delete(`${this.productsBaseUrl}/${id}`);
  }
}

const catalogServices = new CatalogService();
export default catalogServices;
