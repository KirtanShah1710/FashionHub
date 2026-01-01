import { Component } from '@angular/core';
import { OrderStatus } from '../../shared/Enums/OrderStatus.enum';
import { Order } from '../../shared/models/order.model';
import { OrderService } from '../../shared/services/order.service';
import { CurrencyPipe, DatePipe } from '@angular/common';

@Component({
  selector: 'app-my-orders',
  imports: [CurrencyPipe],
  templateUrl: './my-orders.component.html',
  styleUrl: './my-orders.component.css'
})
export class MyOrdersComponent {
  orders: Order[] = [];
  loading = false;

  OrderStatus = OrderStatus;

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.loading = true;
    this.orderService.getMyOrders().subscribe({
      next: (res) => {
        this.orders = res;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  canCancel(status: string): boolean {
    return status === OrderStatus.Pending || status === OrderStatus.Confirmed;
  }

  cancelOrder(orderId: number): void {
    if (!confirm('Are you sure you want to cancel this order?')) return;

    this.orderService.cancelOrder(orderId).subscribe(() => {
      this.loadOrders();
    });
  }
}
