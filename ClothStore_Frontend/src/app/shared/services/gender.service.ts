import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenderCategory } from '../models/gender.model';

@Injectable({ providedIn: 'root' })
export class GenderService {
  private apiUrl = 'http://localhost:5276/api/gendercategory';

  constructor(private http: HttpClient) {}

  getAll(): Observable<GenderCategory[]> {
    return this.http.get<GenderCategory[]>(this.apiUrl);
  }

  getById(id: number): Observable<GenderCategory> {
    return this.http.get<GenderCategory>(`${this.apiUrl}/${id}`);
  }

  create(data: Partial<GenderCategory>): Observable<GenderCategory> {
    return this.http.post<GenderCategory>(this.apiUrl, data);
  }

  update(id: number, data: Partial<GenderCategory>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
