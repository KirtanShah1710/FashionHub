import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthResponse, LoginDto, RegisterDto } from '../../shared/models/auth';
import { Observable } from 'rxjs';
import { JwtPayload } from '../../shared/models/jwt-payload';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = 'http://localhost:5276/api/auth';

  constructor(private http: HttpClient) {}

  // ---- API CALLS (OBSERVABLES) ----

  login(data: LoginDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/login`, data);
  }

  register(data: RegisterDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/register`, data);
  }

  // ---- TOKEN ----

  saveToken(token: string): void {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  logout(): void {
    localStorage.removeItem('token');
  }

  // ---- JWT DECODE ----

  decodeToken(): JwtPayload | null {
    const token = this.getToken();
    return token ? jwtDecode<JwtPayload>(token) : null;
  }

  // ---- PROJECT CRITICAL METHODS ----

  getUserId(): number | null {
    const decoded = this.decodeToken();
    return decoded ? Number(decoded.sub) : null;
  }

  getRole(): string | null {
    const decoded = this.decodeToken();
    return decoded ? decoded.role : null;
  }

  getUserName(): string | null {
    const decoded = this.decodeToken();
    return decoded ? decoded.Name : null;
  }

  // ---- TOKEN VALIDATION ----

  isTokenExpired(): boolean {
    const decoded = this.decodeToken();
    if (!decoded) return true;
    return decoded.exp < Math.floor(Date.now() / 1000);
  }

  isLoggedIn(): boolean {
    return !!this.getToken() && !this.isTokenExpired();
  }
}
