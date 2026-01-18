import { Component } from '@angular/core';
import { CatalogService } from '../../services/catalog.services';
import { CommonModule } from '@angular/common';
import { PagedResult } from '../../../core/models/paged-result.models';
import { ProductForListModel } from '../../models/product-for-list-model';
import { ProductPreviewComponent } from '../product-preview/product-preview.component';
import { RouterLink } from '@angular/router';
import { AppServicesProvider } from '../../../core/services/app-services.provider';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, ProductPreviewComponent, RouterLink],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css',
})
export class ProductListComponent extends AppServicesProvider {
  constructor(private catalogService: CatalogService) {
    super();
  }

  productsPage: PagedResult<ProductForListModel>;
  get isAdminUser() {
    const user = this.authService.getAuthenticatedUser();

    return user && user.isAdmin;
  }

  ngOnInit() {
    this.catalogService
      .getProductsPage({
        pageNumber: 1,
        pageSize: 50,
      })
      .subscribe((page) => {
        this.productsPage = page;
      });
  }
}
