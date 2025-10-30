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
}
