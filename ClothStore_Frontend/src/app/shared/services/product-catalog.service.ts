import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProductCatalogService {
  private baseUrl = 'http://localhost:5276/api/ProductCatalog';
  
  constructor(private http: HttpClient) { }

  getCatalog(filters: any) {
    return this.http.get<any>(this.baseUrl, { params: filters });
  }
}
