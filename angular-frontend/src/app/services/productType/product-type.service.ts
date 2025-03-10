import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductTypeModel } from '../../models/product-type-model';

@Injectable({
  providedIn: 'root'
})
export class ProductTypeService {
  private baseUrl = 'https://localhost:7084/api';

  constructor(private http: HttpClient) { }

  getAllProductTypes(): Observable<ProductTypeModel[]> {
    const url = `${this.baseUrl}/ProductType/getAllProductTypes`;
    return this.http.get<any>(url);
  }
}
