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

  AddToWishlist(userId: Number, productId: Number) {}
  RemoveFromWishlist(userId: Number, productId: Number) {}
  getUserWishlist(userId: Number) {

    return this.http
      .get<WishlistItemModel[]>(
        `${this.baseUrl}/wishlist/getWishlist?userId=${userId}`
      )
      .pipe(
        tap((productId) => this.WishlistItemCountSubject.next(productId.length) )
      );
  }
}
