import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { WishlistItemModel } from '../../models/wishlist-item-model';
import { BehaviorSubject, Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class WishlistService {
  private baseUrl = 'https://localhost:7084/api';
  private WishlistItemCountSubject = new BehaviorSubject<number>(0);
  private wishlistedProducts = new Set<number>();
  private wishlistSubject = new BehaviorSubject<Set<number>>(new Set<number>());

  WishlistItemCount$ = this.WishlistItemCountSubject.asObservable();

  constructor(private http: HttpClient) {}

  addToWishlist(userId: number, productId: number) {
    const body = { userId, productId };
    return this.http.post(`${this.baseUrl}/wishlist/addToWishlist`, body).pipe(
      tap(() => this.refreshWishlistItemCount(userId))
    );
  }

  removeFromWishlist(userId: number, productId: number) {
    return this.http.delete(
      `${this.baseUrl}/wishlist/removeFromWishlist?userId=${userId}&productId=${productId}`).pipe(
    tap(() => this.refreshWishlistItemCount(userId))
    );
  }

  getUserWishlist(userId: number) {
    return this.http
      .get<WishlistItemModel[]>(
        `${this.baseUrl}/wishlist/getWishlist?userId=${userId}`
      )
      .pipe(
        tap((productId) => this.WishlistItemCountSubject.next(productId.length))
      );
  }

  private refreshWishlistItemCount(userId: number): void {
    this.getUserWishlist(userId).subscribe();
  }

   toggleWishlist(userId: number, productId: number, productName: string): void {
    if (this.wishlistedProducts.has(productId)) {
      this.removeFromWishlist(userId, productId).subscribe({
        next: () => {
          this.wishlistedProducts.delete(productId);
          this.wishlistSubject.next(this.wishlistedProducts);
          console.log(`${productName} removed from wishlist`);
        },
        error: (err) => console.error('Error removing from wishlist:', err),
      });
    } else {
      this.addToWishlist(userId, productId).subscribe({
        next: () => {
          this.wishlistedProducts.add(productId);
          this.wishlistSubject.next(this.wishlistedProducts);
          console.log(`${productName} added to wishlist`);
        },
        error: (err) => console.error('Error adding to wishlist:', err),
      });
    }
  }

  loadUserWishlist(userId: number): void {
    this.getUserWishlist(userId).subscribe({
      next: (wishlistItems) => {
        wishlistItems.forEach((item: any) => {
          const id = item.productId ?? item;
          this.wishlistedProducts.add(id);
        });
        this.wishlistSubject.next(this.wishlistedProducts);
      },
      error: (err) => console.error('Error loading wishlist:', err),
    });
  }

    clearWishlist(): void {
    this.wishlistedProducts.clear();
    this.wishlistSubject.next(new Set<number>());
    this.WishlistItemCountSubject.next(0);
  }
}
