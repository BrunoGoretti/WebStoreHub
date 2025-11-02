import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../services/order/order.service';
import { OrderModel } from '../../models/order';
import { AuthService } from '../../services/auth/auth.service';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-order-item',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './order-item.component.html',
  styleUrls: ['./order-item.component.css']
})
export class OrderItemComponent implements OnInit {
  userId: number | null = null;
  orders: OrderModel[] = [];

  constructor(
    private orderService: OrderService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authService.getUserId().subscribe((userId) => {
      if (userId) {
        this.userId = userId;
        this.loadOrders();
      } else {
        console.error('User is not logged in.');
        this.router.navigate(['/login']);
      }
    });
  }

  loadOrders(): void {
    if (this.userId) {
      this.orderService.getOrdersByUserId(this.userId).subscribe((orders) => {
        this.orders = orders.sort((a, b) => {
          return new Date(b.orderDate).getTime() - new Date(a.orderDate).getTime();
        });
        console.log('Sorted Orders:', this.orders);
      });
    }
  }
}
