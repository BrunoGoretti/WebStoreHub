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
  private userId = new BehaviorSubject<number | null>(null);

  constructor(private http: HttpClient) {
    const token = localStorage.getItem('token');
    const username = localStorage.getItem('username');
    const userId = localStorage.getItem('userId');
    if (token) {
      this.isAuthenticated.next(true);
      this.username.next(username);
      this.userId.next(userId ? +userId : null);
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
          localStorage.setItem('userId', response.userId);
          this.isAuthenticated.next(true);
          this.username.next(response.username);
          this.userId.next(response.userId);
          console.log('Token stored:', response.token);
          console.log('Username stored:', response.username);
          console.log('UserId stored:', response.userId);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    localStorage.removeItem('userId');
    this.isAuthenticated.next(false);
    this.username.next(null);
    this.userId.next(null);
  }


  isLoggedIn(): Observable<boolean> {
    return this.isAuthenticated.asObservable();
  }

  getUsername(): Observable<string | null> {
    return this.username.asObservable();
  }

  getUserId(): Observable<number | null> {
    return this.userId.asObservable();
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
