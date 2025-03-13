import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CartItemModel } from '../../models/user-item-model';

@Injectable({
  providedIn: 'root'
})

export class UserItemCartService {

  private baseUrl = 'https://localhost:7084/api';

  constructor(private http: HttpClient) { }

  addToCart(userId: number, productId: number, quantity: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/cart/addToCart`, { userId, productId, quantity });
  }

  updateCartItem(userId: number, cartItemId: number, quantity: number): Observable<any> {
    return this.http.put(`${this.baseUrl}/cart/updateCart/${cartItemId}`, { userId, quantity });
  }

  removeFromCart(userId: number, cartItemId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/cart/removeFromCart/${cartItemId}?userId=${userId}`);
  }

  clearCart(userId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/cart/clearCart?userId=${userId}`);
  }

  getCartItems(userId: number): Observable<CartItemModel[]> {
    return this.http.get<CartItemModel[]>(`${this.baseUrl}/cart/cartItems?userId=${userId}`);
  }
}
