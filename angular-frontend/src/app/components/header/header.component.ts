import { Component, OnInit } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductTypeService } from '../../services/productType/product-type.service';
import { ProductTypeModel } from '../../models/product-type-model';
import { ItemCartService } from '../../services/cartItem/cart-item.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})

export class HeaderComponent implements OnInit {
  username: string | null = '';
  isLoggedIn: boolean = false;
  searchQuery: string = '';
  isCatalogOpen: boolean = false;
  productTypes: ProductTypeModel[] = [];
  cartItemCount: number = 0;

  constructor(
    public authService: AuthService,
    private router: Router,
    private productTypeService: ProductTypeService,
    private ItemCartService: ItemCartService,
  ) {}

  ngOnInit(): void {
    this.authService.isLoggedIn().subscribe((loggedIn) => {
      this.isLoggedIn = loggedIn;
      if (loggedIn) {
        this.loadCartItemCount();
      }
    });

    this.authService.getUsername().subscribe((username) => {
      this.username = username;
    });

    this.productTypeService.getAllProductTypes().subscribe((types) => {
      this.productTypes = types;
    });

    this.ItemCartService.cartItemCount$.subscribe((count) => {
      this.cartItemCount = count;
    });

  }

  loadCartItemCount(): void {
    const userId = Number(localStorage.getItem('userId'));
    if (userId) {
      this.ItemCartService.getCartItems(userId).subscribe();
    }
  }

  logout(): void {
    this.authService.logout();
    this.cartItemCount = 0;
  }


  onSearch(): void {
    if (this.searchQuery.trim()) {
      this.router.navigate(['/search'], { queryParams: { q: this.searchQuery } });
    }
  }

  toggleCatalog(): void {
    this.isCatalogOpen = !this.isCatalogOpen;
  }

  closeCatalog(): void {
    this.isCatalogOpen = false;
  }

  onLogoClick(): void {
    this.router.navigate(['/products']).then(() => {
      window.location.reload();
    });
  }
}
