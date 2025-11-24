import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductBrandModel } from '../../models/product-brand-model';

@Injectable({
  providedIn: 'root'
})
export class ProductBrandService {
private baseUrl = 'https://localhost:7084/api';

  constructor(private http: HttpClient) { }

 addBrand(brandName: string) {
     return this.http.post(`${this.baseUrl}/Brand/addBrand`, {brandName});
 }

  getAllBrands(): Observable<ProductBrandModel[]> {
    const url = `${this.baseUrl}/Brand/getAllBrands`;
    return this.http.get<ProductBrandModel[]>(url);
  }
}
