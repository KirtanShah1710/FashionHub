import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Size } from '../models/size.model';

@Injectable({
  providedIn: 'root'
})
export class SizeService {

  private baseUrl = 'http://localhost:5276/api/Size';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Size[]> {
    return this.http.get<Size[]>(this.baseUrl);
  }

  getById(id: number): Observable<Size> {
    return this.http.get<Size>(`${this.baseUrl}/${id}`);
  }

  create(data: Size): Observable<Size> {
    return this.http.post<Size>(this.baseUrl, data);
  }

  update(id: number, data: Size): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
