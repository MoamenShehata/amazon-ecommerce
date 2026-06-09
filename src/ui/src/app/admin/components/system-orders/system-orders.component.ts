import { Component } from '@angular/core';
import { PagedResult } from '../../../core/models/paged-result.models';
import { AppServicesProvider } from '../../../core/services/app-services.provider';
import { OrderForListDto } from '../../../orders/models/OrderForListDto';
import { SearchOrdersRequest } from '../../../orders/models/search-orders.model';
import { OrdersService } from '../../../orders/orders.services';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-system-orders',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './system-orders.component.html',
  styleUrl: './system-orders.component.css'
})
export class SystemOrdersComponent extends AppServicesProvider {
  constructor(private ordersService: OrdersService) {
    super();
  }

  isLoading = false;

  searchRequest: SearchOrdersRequest = {
    pageNumber: 1,
    pageSize: 100,
    lastSeenValue: null,
  };

  ordersPage: PagedResult<OrderForListDto>;

  get hasNextPage(): boolean {
    return (
      this.ordersPage &&
      this.searchRequest.pageNumber <
      Math.ceil(this.ordersPage.totalCount / this.searchRequest.pageSize)
    );
  }

  get hasPreviousPage(): boolean {
    return this.searchRequest.pageNumber > 1;
  }

  ngOnInit() {
    this.loadOrdersPage(1);
  }

  loadOrdersPage(pageNumber: number): void {
    this.searchRequest.pageNumber = pageNumber;

    this.isLoading = true;

    this.ordersService.getOrdersPageForCurrentUser(this.searchRequest).subscribe({
      next: (page) => {
        this.ordersPage = page;
        this.searchRequest.lastSeenValue = page.lastSeenValue;
        this.isLoading = false;
      },
      error: (err) => {
        console.error("Error loading orders:", err);
        this.isLoading = false;
      },
    });
  }

  nextPage(): void {
    if (this.hasNextPage) {
      this.loadOrdersPage(this.searchRequest.pageNumber + 1);
    }
  }

  previousPage(): void {
    if (this.hasPreviousPage) {
      this.loadOrdersPage(this.searchRequest.pageNumber - 1);
    }
  }

  goToPage(pageNumber: number): void {
    if (
      pageNumber >= 1 &&
      pageNumber <=
      Math.ceil(this.ordersPage.totalCount / this.searchRequest.pageSize)
    ) {
      this.loadOrdersPage(pageNumber);
    }
  }

  getPageNumbers(): number[] {
    const totalPages = Math.ceil(
      this.ordersPage.totalCount / this.searchRequest.pageSize,
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
        this.searchRequest.pageNumber - Math.floor(maxPagesToShow / 2),
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