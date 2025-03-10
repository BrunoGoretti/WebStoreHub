import { Component, OnInit } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductTypeService } from '../../services/productType/product-type.service';
import { ProductTypeModel } from '../../models/product-type-model';

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

  constructor(public authService: AuthService, private router: Router, private productTypeService: ProductTypeService) {}

  ngOnInit(): void {
    this.authService.isLoggedIn().subscribe((loggedIn) => {
      this.isLoggedIn = loggedIn;
    });

    this.authService.getUsername().subscribe((username) => {
      this.username = username;
    });

    this.productTypeService.getAllProductTypes().subscribe((types) => {
      this.productTypes = types;
    });
  }

  logout(): void {
    this.authService.logout();
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
