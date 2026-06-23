import axios from "axios";
import { environment } from "../../environment";
import type { PageRequest } from "../../core/models/page-request.models";
import type { PagedResult } from "../../core/models/paged-result.models";
import type { ProductForListModel } from "../components/models/product-for-list-model";
import type { CategoryForListModel } from "../components/models/category-for-list.models";
import type { ProductCreateRequest } from "../components/models/product-create.model";

export class CatalogService {
  private categoriesBaseUrl = `${environment.gatewayBaseUrl}/categories`;
  private productsBaseUrl = `${environment.gatewayBaseUrl}/products`;

  getCategoriesPage(pageRequest: PageRequest) {
    return axios.get<PagedResult<CategoryForListModel>>(
      `${this.categoriesBaseUrl}?pageNumber=${pageRequest.pageNumber}&pageSize=${pageRequest.pageSize}&lastSeenValue=${pageRequest.lastSeenValue}`,
    );
  }

  createCategory(categoryRequest: { name: string; parentCategoryId?: string | null }) {
    return axios.post<CategoryForListModel>(this.categoriesBaseUrl, categoryRequest);
  }

  getProductsPage(pageRequest: PageRequest) {
    return axios.get<PagedResult<ProductForListModel>>(
      `${this.productsBaseUrl}?pageNumber=${pageRequest.pageNumber}&pageSize=${pageRequest.pageSize}&lastSeenValue=${pageRequest.lastSeenValue}`,
    );
  }

  createProduct(productRequest: ProductCreateRequest, image?: File) {
    const formData = new FormData();
    formData.append('categoryId', productRequest.categoryId);
    formData.append('name', productRequest.name);
    formData.append('inStockCount', productRequest.inStockCount.toString());
    formData.append('price', productRequest.price.toString());
    formData.append('minimumPrice', productRequest.minimumPrice.toString());
    formData.append('maximumPrice', productRequest.maximumPrice.toString());

    // Append properties with proper indexing for ASP.NET Core model binding
    productRequest.properties.forEach((prop, index) => {
      formData.append(`properties[${index}].key`, prop.name);
      formData.append(`properties[${index}].value`, prop.value);
    });

    // Append image if provided
    if (image) {
      formData.append('image', image, image.name);
    }

    return axios.post<{ id: string }>(this.productsBaseUrl, formData);
  }

  deleteProduct(id: string) {
    return axios.delete(`${this.productsBaseUrl}/${id}`)
  }
}

const catalogServices = new CatalogService();
export default catalogServices;