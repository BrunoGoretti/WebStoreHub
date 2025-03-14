import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../services/order/order.service';
import { ItemCartService } from '../../services/cartItem/cart-item.service';
import { OrderModel } from '../../models/order';
import { CartItemModel } from '../../models/cart-item-model';
import { CommonModule } from '@angular/common';
import { DatePipe } from '@angular/common';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css'],
  providers: [DatePipe]

})
export class OrderComponent implements OnInit {
  orders: OrderModel[] = [];
  cartItems: CartItemModel[] = [];
  userId: number | null = null;


  constructor(
    private orderService: OrderService,
    private ItemCartService: ItemCartService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.authService.getUserId().subscribe((userId) => {
      this.userId = userId;
      if (this.userId) {
        this.loadCartItems();
        this.loadOrders();
      }
    });
  }

  loadCartItems(): void {
    if (this.userId) {
      this.ItemCartService.getCartItems(this.userId).subscribe((items) => {
        this.cartItems = items;
      });
    }
  }

  loadOrders(): void {
    if (this.userId) {
      this.orderService.getOrdersByUserId(this.userId).subscribe((orders) => {
        this.orders = orders;
      });
    }
  }

createOrder(): void {
  if (this.userId) {
    this.orderService.createOrder(this.userId).subscribe({
      next: (order) => {
        console.log('Order created:', order);
        this.loadOrders();
      },
      error: (err) => {
        console.error('Error creating order:', err);
      }
    });
  }
}

  updateOrderStatus(orderId: number, newStatus: string): void {
    this.orderService.updateOrderStatus(orderId, newStatus).subscribe(() => {
      console.log('Order status updated');
      this.loadOrders();
    });
  }
}
