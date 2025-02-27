import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = 'https://localhost:7084/api';
  private isAuthenticated = new BehaviorSubject<boolean>(false);
  private username = new BehaviorSubject<string | null>(null);

  constructor(private http: HttpClient) {
    // Initialize authentication state from localStorage
    const token = localStorage.getItem('token');
    const username = localStorage.getItem('username');
    if (token) {
      this.isAuthenticated.next(true);
      this.username.next(username);
    }
  }

  login(email: string, password: string): Observable<any> {
    const params = new HttpParams()
      .set('email', email)
      .set('password', password);

    const url = `${this.baseUrl}/User/login`;
    return this.http.post<any>(url, {}, { params }).pipe(
      tap((response) => {
        if (response.token) {
          localStorage.setItem('token', response.token);
          localStorage.setItem('username', response.username);
          this.isAuthenticated.next(true);
          this.username.next(response.username);
          console.log('Token stored:', response.token);
          console.log('Username stored:', response.username);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    this.isAuthenticated.next(false);
    this.username.next(null);
  }

  isLoggedIn(): Observable<boolean> {
    return this.isAuthenticated.asObservable();
  }

  getUsername(): Observable<string | null> {
    return this.username.asObservable();
  }

  Register(username: string, fullname: string, email: string, password: string): Observable<any> {
    const params = new HttpParams()
      .set('Username', username)
      .set('FullName', fullname)
      .set('Email', email)
      .set('PasswordHash', password);

    const url = `${this.baseUrl}/user/registration`;
    return this.http.post<any>(url, {}, { params }).pipe(
      tap((response) => {
        if (response.token) {
          localStorage.setItem('token', response.token);
          this.isAuthenticated.next(true);
          console.log('Token stored', response.token);
        }
      })
    );
  }
}
