import { Component, OnInit  } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../services/product/product.service';
import { Product } from '../../models/product';
import { CommonModule } from '@angular/common';
import { PaginationComponent } from '../../components/pagination/pagination.component';
import { SortingService } from '../../services/sorting/sorting.service';
import { FilterSortComponent } from '../../components/filter-sort/filter-sort.component';

@Component({
  selector: 'app-search-product',
  standalone: true,
  imports: [CommonModule, FilterSortComponent],
  templateUrl: './search-product.component.html',
  styleUrls: ['./search-product.component.css', '../../components/pagination/pagination.component.css']
})

export class SearchProductComponent extends PaginationComponent implements OnInit {
  searchQuery: string = '';
  originalProducts: Product[] = [];

  constructor(private route: ActivatedRoute, private productService: ProductService, private router: Router, private sortingService: SortingService) {
    super();
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.searchQuery = params['q'];
      if (this.searchQuery) {
        this.productService.searchProductsByName(this.searchQuery).subscribe(
          (data) => {
            this.products = data;
            this.updatePaginatedProducts();
            this.originalProducts = [...data];
          },
          (error) => console.error('Error fetching search results:', error)
        );
      }
    });
  }

  onProductClick(product: Product): void {
    this.router.navigate(['/product', product.productId]);
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
