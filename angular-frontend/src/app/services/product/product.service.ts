import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../../models/product-model';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})

export class ProductService {
  private baseUrl = 'https://localhost:7084/api';

  constructor(private http: HttpClient) {}

  createProductAsync(
    name: string,
    description: string,
    price: number,
    stock: number,
    productTypeId: number,
    brandId: number
  ): Observable<any> {
    return this.http.post(`${this.baseUrl}/Product/addProduct`, { name, description, price, stock, productTypeId, brandId });
  }

  removeProduct(productId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Product/deleteProduct/${productId}`);
  }

  getAllProducts(): Observable<Product[]> {
    const url = `${this.baseUrl}/product/getAllProducts`;
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
          discountPercentage: product.discounts?.[0]?.discountPercentage || 0,
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
        discountPercentage: product.discounts?.[0]?.discountPercentage || 0,
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
          discountPercentage: product.discounts?.[0]?.discountPercentage || 0,
        }));
      })
    );
  }
}
