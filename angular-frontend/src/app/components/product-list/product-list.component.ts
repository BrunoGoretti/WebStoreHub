import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/product/product.service';
import { Product } from '../../models/product-model';
import { HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { PaginationComponent } from '../../components/pagination/pagination.component';
import { SortingService } from '../../services/sorting/sorting.service';
import { FilterSortComponent } from '../../components/filter-sort/filter-sort.component';
import { ProductTypeService } from '../../services/productType/product-type.service';
import { ProductTypeModel } from '../../models/product-type-model';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, HttpClientModule, FilterSortComponent],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css', '../../components/pagination/pagination.component.css']
})
export class ProductListComponent extends PaginationComponent implements OnInit {
  originalProducts: Product[] = [];
  typeName: string = '';
  productTypes: ProductTypeModel[] = [];

  constructor(
    private productService: ProductService,
    private router: Router,
    private sortingService: SortingService,
    private productTypeService: ProductTypeService,) {
    super();
  }

  ngOnInit(): void {
    this.productService.getAllProducts().subscribe((data) => {
      this.products = data;
      this.originalProducts = [...data];
      this.updatePaginatedProducts();
    });
    this.productTypeService.getAllProductTypes().subscribe((data) => {
      this.productTypes = data;
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

  loadProductsByType(typeName: string): void {
    this.productService.getAllProducts().subscribe((data) => {
      this.products = data.filter(product => product.productType?.typeName === typeName);
      this.originalProducts = [...this.products];
      this.currentPage = 1;
      this.updatePaginatedProducts();
    });
  }

  filterByProductType(typeName: string): void {
    this.typeName = typeName;
    this.loadProductsByType(typeName);
  }

  toggleWishlist(product: any, event: MouseEvent) {
  event.stopPropagation(); // prevent product click event
  product.isWishlisted = !product.isWishlisted;
  console.log(product.name, product.isWishlisted ? 'added to wishlist' : 'removed from wishlist');
}
}
