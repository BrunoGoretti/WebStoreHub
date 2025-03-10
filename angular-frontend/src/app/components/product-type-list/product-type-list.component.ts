import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../services/product/product.service';
import { Product } from '../../models/product-model';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { PaginationComponent } from '../../components/pagination/pagination.component';
import { SortingService } from '../../services/sorting/sorting.service';
import { FilterSortComponent } from '../../components/filter-sort/filter-sort.component';

@Component({
  selector: 'app-product-type-list',
  standalone: true,
  imports: [CommonModule, CurrencyPipe, FilterSortComponent],
  templateUrl: './product-type-list.component.html',
  styleUrls: ['./product-type-list.component.css', '../../components/pagination/pagination.component.css']
})
export class ProductTypeListComponent extends PaginationComponent implements OnInit {
  originalProducts: Product[] = [];
  typeName: string = '';
  searchQuery: string = '';

  constructor( private productService: ProductService, private route: ActivatedRoute, private router: Router, private sortingService: SortingService) { super();
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.typeName = params['typeName'];
      this.loadProductsByType(this.typeName);
    });
  }

  loadProductsByType(typeName: string): void {
    this.productService.getAllProducts().subscribe((data) => {
      this.products = data.filter(product => product.productType?.typeName === typeName);
      this.originalProducts = [...this.products];
      this.currentPage = 1;
      this.updatePaginatedProducts();
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
