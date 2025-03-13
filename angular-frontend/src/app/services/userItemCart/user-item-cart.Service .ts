import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { CartItemModel } from '../../models/user-item-model';

@Injectable({
  providedIn: 'root'
})

export class UserItemCartService {

  private baseUrl = 'https://localhost:7084/api';
  private cartItemCountSubject = new BehaviorSubject<number>(0);
  cartItemCount$ = this.cartItemCountSubject.asObservable();

  constructor(private http: HttpClient) { }

  addToCart(userId: number, productId: number, quantity: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/cart/addToCart`, { userId, productId, quantity }).pipe(
      tap(() => this.incrementCartItemCount()) // Increment count
    );
  }

  updateCartItem(userId: number, cartItemId: number, quantity: number): Observable<any> {
    return this.http.put(`${this.baseUrl}/cart/updateCart/${cartItemId}`, { userId, quantity }).pipe(
      tap(() => this.refreshCartItemCount(userId)) // Refresh count
    );
  }

  removeFromCart(userId: number, cartItemId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/cart/removeFromCart/${cartItemId}?userId=${userId}`).pipe(
      tap(() => this.decrementCartItemCount())
    );
  }

  clearCart(userId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/cart/clearCart?userId=${userId}`).pipe(
      tap(() => this.cartItemCountSubject.next(0))
    );
  }

  getCartItems(userId: number): Observable<CartItemModel[]> {
    return this.http.get<CartItemModel[]>(`${this.baseUrl}/cart/cartItems?userId=${userId}`).pipe(
      tap((cartItems) => this.cartItemCountSubject.next(cartItems.length))
    );
  }

  private incrementCartItemCount(): void {
    this.cartItemCountSubject.next(this.cartItemCountSubject.value + 1);
  }

  private decrementCartItemCount(): void {
    this.cartItemCountSubject.next(this.cartItemCountSubject.value - 1);
  }

  private refreshCartItemCount(userId: number): void {
    this.getCartItems(userId).subscribe();
  }
}
