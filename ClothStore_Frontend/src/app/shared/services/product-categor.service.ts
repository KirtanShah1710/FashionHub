import { Injectable } from '@angular/core';
import { ProductCategory } from '../models/ProductCategory';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductCategorService {

  private baseUrl = 'http://localhost:5276/api/ProductCategory';

  constructor(private http: HttpClient) {}


  getAll(): Observable<ProductCategory[]> {
    return this.http.get<ProductCategory[]>(this.baseUrl);
  }

  getById(id: number): Observable<ProductCategory> {
    return this.http.get<ProductCategory>(`${this.baseUrl}/${id}`);
  }

  create(data: ProductCategory): Observable<ProductCategory> {
    return this.http.post<ProductCategory>(this.baseUrl, data);
  }

  update(id: number, data: ProductCategory): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
