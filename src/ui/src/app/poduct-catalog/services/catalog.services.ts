import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class CatalogService {
  categoriesBaseUrl = `${environment.catalogBaseUrl}/categories`;
  productsBaseUrl = `${environment.catalogBaseUrl}/products`;

  constructor(private http: HttpClient) {}

  getProductsPage() {
    return this.http.get<any>(`${this.productsBaseUrl}/`);
  }
}
