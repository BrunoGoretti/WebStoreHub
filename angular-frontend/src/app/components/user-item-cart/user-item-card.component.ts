import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CartItemModel } from '../../models/user-item-model';
import { AuthService } from '../../services/auth/auth.service';
import { UserItemCartService } from '../../services/userItemCart/user-item-cart.Service ';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-item-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-item-card.component.html',
  styleUrl: './user-item-card.component.css'
})
export class UserItemCartComponent implements OnInit {
  cartItems: CartItemModel[] = [];
  userId: number | null = null;

  constructor(
    private UserItemCartService: UserItemCartService,
    private authService: AuthService,
    private router: Router
  ) {}


  ngOnInit(): void {
    this.authService.getUserId().subscribe((userId) => {
      if (userId) {
        this.userId = userId;
        this.loadCartItems();
      } else {
        console.error('User is not logged in.');
        this.router.navigate(['/login']);
      }
    });
  }

  loadCartItems(): void {
    if (this.userId) {
      this.UserItemCartService.getCartItems(this.userId).subscribe(
        (items) => {
          this.cartItems = items;
        },
        (error) => {
          console.error('Error fetching cart items:', error);
        }
      );
    }
  }

  removeItem(cartItemId: number): void {
    if (this.userId) {
      this.UserItemCartService.removeFromCart(this.userId, cartItemId).subscribe(
        () => {
          this.cartItems = this.cartItems.filter(item => item.cartItemId !== cartItemId);
        },
        (error) => {
          console.error('Error removing item from cart:', error);
        }
      );
    }
  }

  clearCart(): void {
    if (this.userId) {
      this.UserItemCartService.clearCart(this.userId).subscribe(
        () => {
          this.cartItems = [];
        },
        (error) => {
          console.error('Error clearing cart:', error);
        }
      );
    }
  }

}
