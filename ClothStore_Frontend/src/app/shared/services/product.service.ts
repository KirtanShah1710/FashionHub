import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private baseUrl = 'http://localhost:5276/api/Product';

  constructor(private http: HttpClient) {}

  // ================= CREATE =================
  create(formData: FormData): Observable<any> {
    return this.http.post<any>(this.baseUrl, formData);
  }

  // ================= GET ALL =================
  getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl);
  }

  // ================= GET BY ID =================
  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${id}`);
  }

  // ================= UPDATE =================
  update(id: number, formData: FormData): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/${id}`, formData);
  }

  // ================= DELETE =================
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
