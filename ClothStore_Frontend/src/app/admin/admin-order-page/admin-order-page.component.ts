import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { Order } from '../../shared/models/order.model';
import { OrderService } from '../../shared/services/order.service';
import { OrderStatus } from '../../shared/Enums/OrderStatus.enum';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-order-page',
  standalone: true,
  imports: [FormsModule, DatePipe, CurrencyPipe],
  templateUrl: './admin-order-page.component.html',
  styleUrls: ['./admin-order-page.component.css']
})
export class AdminOrderPageComponent implements OnInit {
  orders: Order[] = [];
  filteredOrders: Order[] = [];

  OrderStatus = OrderStatus;

  statuses = ['Pending', 'Confirmed', 'Shipped', 'Delivered', 'Cancelled'];
  paymentStatuses = ['Pending', 'Paid'];

  selectedStatusFilter: string = 'All';
  selectedPaymentFilter: string = 'All';

  loading = false;

  constructor(private orderService: OrderService,
    private toaster: ToastrService
  ) {}

  ngOnInit(): void {
    this.getAllOrders();
  }

  getAllOrders(): void {
    this.loading = true;

    this.orderService.getAllOrders().subscribe({
      next: (res) => {
        this.orders = res;
        this.filteredOrders = res;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  applyFilter(): void {
    this.filteredOrders = this.orders.filter(order => {
      const orderStatusMatch =
        this.selectedStatusFilter === 'All' ||
        order.status === this.selectedStatusFilter;

      const paymentStatusMatch =
        this.selectedPaymentFilter === 'All' ||
        order.paymentStatus === this.selectedPaymentFilter;

      return orderStatusMatch && paymentStatusMatch;
    });
  }

  updateStatus(order: Order, newStatus: string): void {
    this.orderService.updateOrderStatus(order.orderId, newStatus).subscribe({
      next: () => {
        order.status = newStatus;
        this.applyFilter(); // re-apply filters
        this.toaster.success(`Order status updated to ${newStatus}`);
      },
      error: (err) => {
        console.error(err);
        this.toaster.error('Failed to update order status');
      }
    });
  }

  onSelectChange(order: Order, event: Event): void {
    const newStatus = (event.target as HTMLSelectElement).value;
    this.updateStatus(order, newStatus);
  }
}
