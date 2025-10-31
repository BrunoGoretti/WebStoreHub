import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WishlistItemModel } from '../../models/wishlist-item-model';
import { WishlistService } from '../../services/wishlist/wishlist.service';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-wishlist-item',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './wishlist-item.component.html',
  styleUrl: './wishlist-item.component.css',
})
export class WishlistItemComponent {
  WishlistItems: WishlistItemModel[] = [];
  userId: number | null = null;

  constructor(
    private WishlistService: WishlistService,
    private router: Router,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    this.authService.getUserId().subscribe((userId) => {
      if(userId) {
        this.userId = userId;
        this.loadUserWishlist();
      } else {
        console.error('User is not logged in.');
        this.router.navigate(['/login']);
      }
    })
  }

  addTomWishlist(){}

  removeFromWishlist(){}

  loadUserWishlist(): void {
    if (this.userId) {
      this.WishlistService.getUserWishlist(this.userId).subscribe(
        (items) => {
          this.WishlistItems = items;
        },
        (error) => {
          console.error('Error fetching cart items:', error);
        }
      );
    }
  }

}
