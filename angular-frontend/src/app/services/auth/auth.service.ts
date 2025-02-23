import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = 'https://localhost:7084/api';

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<any> {
    // Set query parameters
    const params = new HttpParams()
      .set('Email', email)
      .set('PasswordHash', password);

    const url = `${this.baseUrl}/User/login`;
    return this.http.post<any>(url, {}, { params }).pipe(
      tap((response) => {
        if (response.token) {
          localStorage.setItem('token', response.token); // Store the token
          console.log('Token stored:', response.token); // Debugging
        }
      })
    );
  }
}
