import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductVariantService {

  private apiUrl = 'http://localhost:5276/api/ProductVariant';

  constructor(private http: HttpClient) {}

  // ---------- GET ALL ----------
  getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  // ---------- GET BY ID ----------
  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  // ---------- GET BY PRODUCT ID ----------
  getByProductId(productId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/product/${productId}`);
  }

  // ---------- CREATE ----------
  create(formData: FormData): Observable<string> {
    return this.http.post(this.apiUrl, formData, {
      responseType: 'text'
    });
  }

  // ---------- UPDATE ----------
  update(id: number, formData: FormData): Observable<string> {
    return this.http.put(`${this.apiUrl}/${id}`, formData, {
      responseType: 'text'
    });
  }

  // ---------- DELETE ----------
  delete(id: number): Observable<string> {
    return this.http.delete(`${this.apiUrl}/${id}`, {
      responseType: 'text'
    });
  }
}
