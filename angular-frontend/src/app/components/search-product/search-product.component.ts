import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../services/product/product.service';
import { Product } from '../../models/product-model';
import { CommonModule } from '@angular/common';
import { PaginationComponent } from '../../components/pagination/pagination.component';
import { SortingService } from '../../services/sorting/sorting.service';
import { FilterSortComponent } from '../../components/filter-sort/filter-sort.component';
import { WishlistService } from '../../services/wishlist/wishlist.service';
import { AuthService } from '../../services/auth/auth.service';
import { PaginationStateService } from '../../services/pagination/pagination-state.service';

@Component({
  selector: 'app-search-product',
  standalone: true,
  imports: [CommonModule, FilterSortComponent],
  templateUrl: './search-product.component.html',
  styleUrls: [
    './search-product.component.css',
    '../../components/pagination/pagination.component.css',
  ],
})
export class SearchProductComponent
  extends PaginationComponent
  implements OnInit
{
  searchQuery: string = '';
  previousQuery: string = '';
  originalProducts: Product[] = [];
  wishlistedProducts = new Set<number>();
  userId: number | null = null;
  isLoggedIn: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private router: Router,
    private sortingService: SortingService,
    private wishlistService: WishlistService,
    private authService: AuthService,
    paginationState: PaginationStateService

  ) {
    super(paginationState);
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      const newQuery = params['q'];

      if(newQuery !== this.previousQuery) {
        this.previousQuery = newQuery;
        this.searchQuery = newQuery;

        this.productService.searchProductsByName(newQuery).subscribe(
          (data) => {
            this.products = data;
            this.originalProducts = [...data];
            this.updatePaginatedProducts();
          },
          (error) => console.error('Error fetching search results:', error)
        );
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

    this.authService.isLoggedIn().subscribe((loggedIn) => {
      this.isLoggedIn = loggedIn;
      if (loggedIn) {
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
      this.products = this.sortingService.sortProducts(
        this.products,
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
