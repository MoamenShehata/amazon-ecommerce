import { Component } from '@angular/core';
import { CatalogService } from '../../services/catalog.services';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css',
})
export class ProductListComponent {
  constructor(private catalogService: CatalogService) {}

  products: any[] = [];

  ngOnInit() {
    this.catalogService
      .getProductsPage({
        pageNumber: 1,
        pageSize: 50,
      })
      .subscribe((page) => {
        this.products = page.items;
      });
  }
}
