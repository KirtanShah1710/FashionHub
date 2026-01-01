import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { OrderStatus } from "../Enums/OrderStatus.enum";
import { Order } from "../models/order.model";

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private baseUrl = 'http://localhost:5276/api/Order';

  // Mapping enum strings to numeric values
  private enumMapping: { [key: string]: number } = {
    Pending: 0,
    Confirmed: 1,
    Shipped: 2,
    Delivered: 3,
    Cancelled: 4
  };

  constructor(private http: HttpClient) {}

  // ================= PAYMENT =================
  startPayment(payload: any) {
    return this.http.post<any>(`${this.baseUrl}/payment/start`, payload);
  }

  verifyPayment(payload: any) {
    return this.http.post<any>(`${this.baseUrl}/payment/verify`, payload);
  }

  // ================= CUSTOMER =================
  getMyOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.baseUrl}/my`);
  }

  cancelOrder(orderId: number): Observable<any> {
    return this.http.put(`${this.baseUrl}/${orderId}/cancel`, {});
  }

  // ================= ADMIN =================
  getAllOrders(status?: OrderStatus): Observable<Order[]> {
    let params = new HttpParams();
    if (status) {
      params = params.set('status', status);
    }
    return this.http.get<Order[]>(`${this.baseUrl}/admin`, { params });
  }

  updateOrderStatus(orderId: number, status: string): Observable<any> {
    const numericStatus = this.enumMapping[status]; // use 'this'
    return this.http.put(`${this.baseUrl}/admin/${orderId}/status`, { status: numericStatus });
  }
}
