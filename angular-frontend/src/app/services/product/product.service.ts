import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../../models/product';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private baseUrl = 'https://localhost:7084/api';

  constructor(private http: HttpClient) {}

  getAllProducts(): Observable<Product[]> {
    const url = `${this.baseUrl}/Product/getAllProducts`;
    return this.http.get<any>(url).pipe(
      map((response) => {
        const products = response?.$values || response;
        if (!Array.isArray(products)) {
          throw new Error('Unexpected API response format');
        }
        return products.map((product: any) => ({
          ...product,
          brand: product.brand?.brandName || 'Unknown',
          imageUrl: product.images?.[0]?.imageUrl || '',
          discountedPrice: product.discountedPrice || null,
        }));
      })
    );
  }

  getProductById(productId: number): Observable<Product> {
    const url = `${this.baseUrl}/Product/getProductById/${productId}`;
    return this.http.get<any>(url).pipe(
      map((product) => ({
        ...product,
        brand: {
          brandId: product.brand?.brandId || 0,
          brandName: product.brand?.brandName || 'Unknown',
        },
        imageUrl: product.images?.[0]?.imageUrl || '',
        images: product.images || [],
      }))
    );
  }

  searchProductsByName(name: string): Observable<Product[]> {
    const url = `${
      this.baseUrl
    }/Product/searchProductsByName?name=${encodeURIComponent(name)}`;
    return this.http.get<any[]>(url).pipe(
      map((products) => {
        return products.map((product) => ({
          ...product,
          brand: product.brand?.brandName || 'Unknown',
          imageUrl: product.images?.[0]?.imageUrl || '',
        }));
      })
    );
  }
}
