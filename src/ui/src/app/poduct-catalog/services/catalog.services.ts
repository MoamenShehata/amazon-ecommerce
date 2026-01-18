import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { PageRequest } from '../../core/models/page-request.models';
import { ProductForListModel } from '../models/product-for-list-model';
import { map } from 'rxjs';
import { PagedResult } from '../../core/models/paged-result.models';
import { ProductCreateRequest } from '../models/product-create.model';

@Injectable({
  providedIn: 'root',
})
export class CatalogService {
  categoriesBaseUrl = `${environment.catalogBaseUrl}/categories`;
  productsBaseUrl = `${environment.catalogBaseUrl}/products`;

  constructor(private http: HttpClient) {}

  getProductsPage(pageRequest: PageRequest) {
    return this.http.get<PagedResult<ProductForListModel>>(
      `${this.productsBaseUrl}?pageNumber=${pageRequest.pageNumber}&pageSize=${pageRequest.pageSize}&lastSeenValue=${pageRequest.lastSeenValue}`,
    );
  }

  createProduct(productRequest: ProductCreateRequest) {
    return this.http.post<{ id: string }>(this.productsBaseUrl, productRequest);
  }
}
