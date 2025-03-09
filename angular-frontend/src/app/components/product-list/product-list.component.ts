import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/product/product.service';
import { Product } from '../../models/product';
import { HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { PaginationComponent } from '../../components/pagination/pagination.component';
import { SortingService } from '../../services/sorting/sorting.service';
import { FilterSortComponent } from '../../components/filter-sort/filter-sort.component';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, HttpClientModule, FilterSortComponent],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css', '../../components/pagination/pagination.component.css']
})
export class ProductListComponent extends PaginationComponent implements OnInit {
  originalProducts: Product[] = [];

  constructor(private productService: ProductService, private router: Router, private sortingService: SortingService) {
    super();
  }

  ngOnInit(): void {
    this.productService.getAllProducts().subscribe((data) => {
      this.products = data;
      this.originalProducts = [...data];
      this.updatePaginatedProducts();
    });
  }

  onProductClick(product: Product): void {
    this.router.navigate(['/product', product.productId]);
    console.log("Product clicked:", product);
  }

  onSortChange(sortOption: string): void {
    if (sortOption === '') {
      this.products = [...this.originalProducts];
    } else {
      this.products = this.sortingService.sortProducts(this.products, sortOption);
    }
    this.currentPage = 1;
    this.updatePaginatedProducts();
  }
}
