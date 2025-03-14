import { Injectable } from '@angular/core';
import { HttpClient, HttpParams  } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderModel } from '../../models/order';
import { CartItemModel } from '../../models/cart-item-model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private baseUrl = 'https://localhost:7084/api';

  constructor(private http: HttpClient) {}

  createOrder(userId: number): Observable<OrderModel> {
    const params = new HttpParams().set('userId', userId.toString());
    return this.http.post<OrderModel>(`${this.baseUrl}/order/create`, null, { params });
  }

  getOrderById(orderId: number): Observable<OrderModel> {
    return this.http.get<OrderModel>(`${this.baseUrl}/order/orders/${orderId}`);
  }

  getOrdersByUserId(userId: number): Observable<OrderModel[]> {
    return this.http.get<OrderModel[]>(`${this.baseUrl}/order/user/${userId}`);
}

  updateOrderStatus(orderId: number, newStatus: string): Observable<any> {
    return this.http.put(`${this.baseUrl}/${orderId}/order/updateStatus`, { newStatus });
  }
}
