import { Component } from '@angular/core';
import { CatalogService } from '../../services/catalog.services';
import { CommonModule } from '@angular/common';
import { PagedResult } from '../../../core/models/paged-result.models';
import { ProductForListModel } from '../../models/product-for-list-model';
import { ProductPreviewComponent } from '../product-preview/product-preview.component';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, ProductPreviewComponent],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css',
})
export class ProductListComponent {
  constructor(private catalogService: CatalogService) {}

  productsPage: PagedResult<ProductForListModel>;

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
