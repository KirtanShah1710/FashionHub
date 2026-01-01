import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Color } from '../models/color.model';

@Injectable({
  providedIn: 'root'
})
export class ColorService {

  private baseUrl = 'http://localhost:5276/api/Color';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Color[]> {
    return this.http.get<Color[]>(this.baseUrl);
  }

  getById(id: number): Observable<Color> {
    return this.http.get<Color>(`${this.baseUrl}/${id}`);
  }

  create(data: Color): Observable<Color> {
    return this.http.post<Color>(this.baseUrl, data);
  }

  update(id: number, data: Color): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
