import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ProductService } from '../../services/product/product.service';
import { Product } from '../../models/product-model';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { PaginationComponent } from '../../components/pagination/pagination.component';
import { SortingService } from '../../services/sorting/sorting.service';
import { FilterSortComponent } from '../../components/filter-sort/filter-sort.component';
import { ProductTypeService } from '../../services/productType/product-type.service';
import { ProductTypeModel } from '../../models/product-type-model';
import { PaginationStateService } from '../../services/pagination/pagination-state.service';
import { AuthService } from '../../services/auth/auth.service';
import { WishlistService } from '../../services/wishlist/wishlist.service';

@Component({
  selector: 'app-product-type-list',
  standalone: true,
  imports: [CommonModule, CurrencyPipe, FilterSortComponent, RouterModule],
  templateUrl: './product-type-list.component.html',
  styleUrls: [
    './product-type-list.component.css',
    '../../components/pagination/pagination.component.css',
  ],
})
export class ProductTypeListComponent
  extends PaginationComponent
  implements OnInit
{
  originalProducts: Product[] = [];
  typeName: string = '';
  searchQuery: string = '';
  productTypes: ProductTypeModel[] = [];
  isLoggedIn: boolean = false;
  userId: number | null = null;
  wishlistedProducts = new Set<number>();

  constructor(
    private productService: ProductService,
    private productTypeService: ProductTypeService,
    private route: ActivatedRoute,
    private router: Router,
    private sortingService: SortingService,
    private authService: AuthService,
    private wishlistService: WishlistService,
    paginationState: PaginationStateService
  ) {
    super(paginationState);
  }

  ngOnInit(): void {
    this.paginationState.currentSort$.subscribe((sortOption) => {
      if (sortOption) {
        this.onSortChange(sortOption);
      }
    });

    this.route.params.subscribe((params) => {
      this.typeName = params['typeName'];
      this.loadProductsByType(this.typeName);
    });

    this.productTypeService.getAllProductTypes().subscribe((data) => {
      this.productTypes = data;
    });

     this.authService.isLoggedIn().subscribe((loggedIn) => {
      this.isLoggedIn = loggedIn;
      if (loggedIn) {
      }
    });

     this.authService.getUserId().subscribe((userId) => {
      if (userId) {
        this.userId = userId;
        this.wishlistService.loadUserWishlist(userId);

        this.wishlistService['wishlistSubject'].subscribe(
          (wishlistSet: Set<number>) => {
            this.wishlistedProducts = new Set(wishlistSet);
          }
        );
      }
    });
  }

  loadProductsByType(typeName: string): void {
    this.productService.getAllProducts().subscribe((data) => {
      this.products = data.filter((p) => p.productType?.typeName === typeName);
      this.originalProducts = [...this.products];

      this.paginationState.setPage(1);
      this.updatePaginatedProducts();
    });
  }

  onProductClick(product: Product): void {
    this.router.navigate(['/product', product.productId]);
  }

  onTypeClick(typeName: string) {
    this.router.navigate(['/category', typeName], {
      queryParamsHandling: 'preserve',
    });
  }

  onSortChange(sortOption: string): void {
    if (sortOption === this.paginationState.currentSortSubject.value) return;

    this.paginationState.setSort(sortOption);

    if (sortOption === '') {
      this.products = [...this.originalProducts];
    } else {
      this.products = this.sortingService.sortProducts(
        [...this.originalProducts],
        sortOption
      );
    }
    this.paginationState.setPage(1);
    this.updatePaginatedProducts();
  }

    toggleWishlist(product: Product, event: MouseEvent) {
    event.stopPropagation();

    if (this.userId == null) {
      console.warn('User not logged in.');
      return;
    }

    this.wishlistService.toggleWishlist(
      this.userId,
      product.productId,
      product.name
    );
  }
}
