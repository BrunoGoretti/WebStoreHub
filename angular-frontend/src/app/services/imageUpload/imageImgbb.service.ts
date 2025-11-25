import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ImageUploadService {
  private baseUrl = 'https://localhost:7084/api';

  constructor(private http: HttpClient ) { }

  addImage(productId: number, file: File, mainImage: number) {
    const formData = new FormData();

    formData.append("Image", file);
    formData.append("ProductId", productId.toString());
    formData.append("MainPicture", mainImage.toString());

    return this.http.post(`${this.baseUrl}/Imgbb/AddImage`, formData);
  }
}
