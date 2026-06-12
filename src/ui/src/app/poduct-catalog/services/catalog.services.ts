import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { PageRequest } from '../../core/models/page-request.models';
import { ProductForListModel } from '../models/product-for-list-model';
import { map } from 'rxjs';
import { PagedResult } from '../../core/models/paged-result.models';
import { ProductCreateRequest } from '../models/product-create.model';
import { CategoryForListModel } from '../models/category-for-list.models';

@Injectable({
  providedIn: 'root',
})
export class CatalogService {
  private categoriesBaseUrl = `${environment.gatewayBaseUrl}/categories`;
  private productsBaseUrl = `${environment.gatewayBaseUrl}/products`;

  constructor(private http: HttpClient) { }

  getCategoriesPage(pageRequest: PageRequest) {
    return this.http.get<PagedResult<CategoryForListModel>>(
      `${this.categoriesBaseUrl}?pageNumber=${pageRequest.pageNumber}&pageSize=${pageRequest.pageSize}&lastSeenValue=${pageRequest.lastSeenValue}`,
    );
  }

  createCategory(categoryRequest: { name: string; parentCategoryId?: string | null }) {
    return this.http.post<CategoryForListModel>(this.categoriesBaseUrl, categoryRequest);
  }

  getProductsPage(pageRequest: PageRequest) {
    return this.http.get<PagedResult<ProductForListModel>>(
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

    return this.http.post<{ id: string }>(this.productsBaseUrl, formData);
  }

  deleteProduct(id: string) {
    return this.http.delete(`${this.productsBaseUrl}/${id}`)
  }
}
