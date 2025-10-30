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
import { WishlistService } from '../../services/wishlist/wishlist.service';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, HttpClientModule, FilterSortComponent],
  templateUrl: './product-list.component.html',
  styleUrls: [
    './product-list.component.css',
    '../../components/pagination/pagination.component.css',
  ],
})
export class ProductListComponent
  extends PaginationComponent
  implements OnInit
{
  originalProducts: Product[] = [];
  typeName: string = '';
  productTypes: ProductTypeModel[] = [];
  userId: number | null = null;
  wishlistedProducts = new Set<number>();

  constructor(
    private productService: ProductService,
    private router: Router,
    private sortingService: SortingService,
    private productTypeService: ProductTypeService,
    private wishlistService: WishlistService,
    private authService: AuthService
  ) {
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

    this.authService.getUserId().subscribe((userId) => {
      if (userId) {
        this.userId = userId;
        // ✅ Load wishlist items from backend
        this.loadUserWishlist();
      }
    });
  }

  onProductClick(product: Product): void {
    this.router.navigate(['/product', product.productId]);
    console.log('Product clicked:', product);
  }

  onSortChange(sortOption: string): void {
    if (sortOption === '') {
      this.products = [...this.originalProducts];
    } else {
      this.products = this.sortingService.sortProducts(
        this.products,
        sortOption
      );
    }
    this.currentPage = 1;
    this.updatePaginatedProducts();
  }

  loadProductsByType(typeName: string): void {
    this.productService.getAllProducts().subscribe((data) => {
      this.products = data.filter(
        (product) => product.productType?.typeName === typeName
      );
      this.originalProducts = [...this.products];
      this.currentPage = 1;
      this.updatePaginatedProducts();
    });
  }

  filterByProductType(typeName: string): void {
    this.typeName = typeName;
    this.loadProductsByType(typeName);
  }

toggleWishlist(product: Product, event: MouseEvent) {
  event.stopPropagation();

  if (this.userId == null) {
    console.warn('User not logged in.');
    return;
  }

  if (this.wishlistedProducts.has(product.productId)) {
    this.wishlistService.removeFromWishlist(this.userId, product.productId).subscribe({
      next: () => {
        this.wishlistedProducts.delete(product.productId);
        console.log(`${product.name} removed from wishlist`);
      },
      error: (err) => console.error('Error removing from wishlist:', err),
    });
  } else {
    this.wishlistService.addToWishlist(this.userId, product.productId).subscribe({
      next: () => {
        this.wishlistedProducts.add(product.productId);
        console.log(`${product.name} added to wishlist`);
      },
      error: (err) => console.error('Error adding to wishlist:', err),
    });
  }
}

  private loadUserWishlist(): void {
  if (!this.userId) return;

  this.wishlistService.getUserWishlist(this.userId).subscribe({
    next: (wishlistItems) => {
      wishlistItems.forEach((item: any) => {
        const productId = item.productId ?? item;
        this.wishlistedProducts.add(productId);
      });
      console.log('Wishlist loaded:', this.wishlistedProducts);
    },
    error: (err) => console.error('Error loading wishlist:', err),
  });
}
}
