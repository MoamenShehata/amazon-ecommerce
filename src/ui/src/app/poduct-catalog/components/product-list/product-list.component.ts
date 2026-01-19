import { Component } from '@angular/core';
import { CatalogService } from '../../services/catalog.services';
import { CommonModule } from '@angular/common';
import { PagedResult } from '../../../core/models/paged-result.models';
import { ProductForListModel } from '../../models/product-for-list-model';
import { ProductPreviewComponent } from '../product-preview/product-preview.component';
import { RouterModule } from '@angular/router';
import { AppServicesProvider } from '../../../core/services/app-services.provider';
import { PageRequest } from '../../../core/models/page-request.models';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, ProductPreviewComponent, RouterModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css',
})
export class ProductListComponent extends AppServicesProvider {
  constructor(private catalogService: CatalogService) {
    super();
  }

  productsPage: PagedResult<ProductForListModel>;

  pageRequest: PageRequest = {
    pageNumber: 1,
    pageSize: 100,
    lastSeenValue: null,
  };

  isLoading = false;

  get isAdminUser() {
    const user = this.authService.getAuthenticatedUser();

    return user && user.isAdmin;
  }

  get hasNextPage(): boolean {
    return (
      this.productsPage &&
      this.pageRequest.pageNumber <
        Math.ceil(this.productsPage.totalCount / this.pageRequest.pageSize)
    );
  }

  get hasPreviousPage(): boolean {
    return this.pageRequest.pageNumber > 1;
  }

  ngOnInit() {
    this.loadProductsPage(1);
  }

  loadProductsPage(pageNumber: number): void {
    this.pageRequest.pageNumber = pageNumber;

    this.isLoading = true;

    this.catalogService.getProductsPage(this.pageRequest).subscribe({
      next: (page) => {
        this.productsPage = page;
        this.pageRequest.lastSeenValue = page.lastSeenValue;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading products:', err);
        this.isLoading = false;
      },
    });
  }

  nextPage(): void {
    if (this.hasNextPage) {
      this.loadProductsPage(this.pageRequest.pageNumber + 1);
    }
  }

  previousPage(): void {
    if (this.hasPreviousPage) {
      this.loadProductsPage(this.pageRequest.pageNumber - 1);
    }
  }

  goToPage(pageNumber: number): void {
    if (
      pageNumber >= 1 &&
      pageNumber <=
        Math.ceil(this.productsPage.totalCount / this.pageRequest.pageSize)
    ) {
      this.loadProductsPage(pageNumber);
    }
  }

  navigateToCreateProduct(): void {
    this.router.navigate(['/catalog/products/create']);
  }

  getPageNumbers(): number[] {
    const totalPages = Math.ceil(
      this.productsPage.totalCount / this.pageRequest.pageSize,
    );
    const pages: number[] = [];
    const maxPagesToShow = 5;

    if (totalPages <= maxPagesToShow) {
      for (let i = 1; i <= totalPages; i++) {
        pages.push(i);
      }
    } else {
      const startPage = Math.max(
        1,
        this.pageRequest.pageNumber - Math.floor(maxPagesToShow / 2),
      );
      const endPage = Math.min(totalPages, startPage + maxPagesToShow - 1);

      if (startPage > 1) {
        pages.push(1);
        if (startPage > 2) {
          pages.push(-1);
        }
      }

      for (let i = startPage; i <= endPage; i++) {
        pages.push(i);
      }

      if (endPage < totalPages) {
        if (endPage < totalPages - 1) {
          pages.push(-1);
        }
        pages.push(totalPages);
      }
    }

    return pages;
  }

  Math = Math;
}
