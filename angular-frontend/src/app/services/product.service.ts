import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private baseUrl  = 'https://localhost:7084/api';

  constructor(private http: HttpClient) {}

  getAllProducts(): Observable<Product[]> {
    const url = `${this.baseUrl}/Product/getAllProducts`;
    return this.http.get<any>(url).pipe(
      map(response => response.$values)
    );
  }
}