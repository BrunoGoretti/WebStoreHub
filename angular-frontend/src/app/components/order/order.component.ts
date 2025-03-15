import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../services/order/order.service';
import { ItemCartService } from '../../services/cartItem/cart-item.service';
import { OrderModel } from '../../models/order';
import { CartItemModel } from '../../models/cart-item-model';
import { CommonModule } from '@angular/common';
import { DatePipe } from '@angular/common';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';

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
  totalCost: number = 0;

  constructor(
    private orderService: OrderService,
    private ItemCartService: ItemCartService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authService.getUserId().subscribe((userId) => {
      this.userId = userId;
      if (this.userId) {
        this.loadCartItems();
      }
    });
  }

  loadCartItems(): void {
    if (this.userId) {
      this.ItemCartService.getCartItems(this.userId).subscribe((items) => {
        this.cartItems = items;
        this.calculateTotalCost();
      });
    }
  }

  calculateTotalCost(): void {
    this.totalCost = this.cartItems.reduce((total, item) => {
      const price = item.product.discountedPrice || item.product.price;
      return total + (price * item.quantity);
    }, 0);
  }

  createOrder(): void {
    if (this.userId) { // Ensure userId is not null
      this.orderService.createOrder(this.userId).subscribe({
        next: (order) => {
          console.log('Order created:', order);
          this.ItemCartService.clearCart(this.userId!).subscribe(() => {
            this.router.navigate(['/orderitem']);
          });
        },
        error: (err) => {
          console.error('Error creating order:', err);
        }
      });
    } else {
      console.error('User ID is null. Cannot create order.');
    }
  }

  updateOrderStatus(orderId: number, newStatus: string): void {
    this.orderService.updateOrderStatus(orderId, newStatus).subscribe(() => {
      console.log('Order status updated');
    });
  }
}
