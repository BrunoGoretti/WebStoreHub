import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';
import { NgIf, NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Product } from '../../models/product-model';
import { ProductTypeService } from '../../services/productType/product-type.service';
import { ProductTypeModel } from '../../models/product-type-model';
import {ProductBrandService} from '../../services/productBrand/product-brand.service'
import { ProductBrandModel } from '../../models/product-brand-model';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [NgIf, FormsModule, NgFor],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css',
})
export class AdminDashboardComponent implements OnInit {
  userId: number | null = null;
  showAddProductForm = false;

  productTypes: ProductTypeModel[] = [];
  selectedType: number | null = null;

  productBrands: ProductBrandModel[] = [];
  selectedBrand: number | null = null;


  constructor(
    private authService: AuthService,
    private router: Router,
    private productTypeService: ProductTypeService,
    private productBrandService: ProductBrandService
  ) {}

  ngOnInit(): void {
    this.authService.isLoggedIn().subscribe((islogged) => {
      if (!islogged) {
        console.warn('User not logged in.');
        this.router.navigate(['/login']);
      }
    });
    this.productTypeService.getAllProductTypes().subscribe((data) => {
      this.productTypes = data;

      console.log((this.productTypes = data));
    });
        this.productBrandService.getAllBrands().subscribe((data) => {
      this.productBrands = data;

      console.log((this.productBrands = data));
    });
  }

  toggleAddProductForm() {
    this.showAddProductForm = !this.showAddProductForm;
  }

  onSubmitAddProduct(event: Event) {
    event.preventDefault();
    this.toggleAddProductForm();
  }
}
