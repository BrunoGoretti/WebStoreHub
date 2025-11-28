import { Component } from '@angular/core';
import { Product } from '../../models/product-model';
import { Subscription } from 'rxjs';
import { PaginationStateService } from '../../services/pagination/pagination-state.service';

@Component({
  template: ''
})

export abstract class PaginationComponent {
  products: Product[] = [];
  paginatedProducts: Product[] = [];
  currentPage: number = 1;
  itemsPerPage: number = 5;

  private pageSub?: Subscription;

  constructor(public paginationState: PaginationStateService) {
    this.pageSub = this.paginationState.currentPage$.subscribe((page) => {
      this.currentPage = page;
      this.updatePaginatedProducts();
    })
  }

  ngOnDestroy(): void {
    this.pageSub?.unsubscribe();
  }


  updatePaginatedProducts(): void {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedProducts = this.products.slice(startIndex, endIndex);
  }

  changePage(page: number | string): void {
    if (typeof page === 'number' && page >= 1 && page <= this.totalPages) {
      this.paginationState.setPage(page);
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.paginationState.setPage(this.currentPage + 1);
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.paginationState.setPage(this.currentPage - 1);
    }
  }

  get totalPages(): number {
    return Math.ceil(this.products.length / this.itemsPerPage);
  }

  get pages(): (number | string)[] {
    const totalPages = this.totalPages;
    const currentPage = this.currentPage;
    const maxVisiblePages = 5;

    if (totalPages <= maxVisiblePages) {
      return Array.from({ length: totalPages }, (_, i) => i + 1);
    } else {
      const pages: (number | string)[] = [];
      const halfVisible = Math.floor(maxVisiblePages / 2);

      pages.push(1);

      if (currentPage > halfVisible + 1) {
        pages.push('...');
      }

      const start = Math.max(2, currentPage - halfVisible);
      const end = Math.min(totalPages - 1, currentPage + halfVisible);

      for (let i = start; i <= end; i++) {
        pages.push(i);
      }

      if (currentPage < totalPages - halfVisible - 1) {
        pages.push('...');
      }

      pages.push(totalPages);

      return pages;
    }
  }
}
