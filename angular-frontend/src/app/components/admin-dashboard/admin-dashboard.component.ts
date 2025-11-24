import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';
import { NgIf, NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductTypeService } from '../../services/productType/product-type.service';
import { ProductTypeModel } from '../../models/product-type-model';
import { ProductBrandService } from '../../services/productBrand/product-brand.service';
import { ProductBrandModel } from '../../models/product-brand-model';
import { Product } from '../../models/product-model';
import { ProductService } from '../../services/product/product.service';
import { Observable } from 'rxjs';

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
  showBrandNameForm = false;

  productTypes: ProductTypeModel[] = [];
  selectedType: number | null = null;

  productBrands: ProductBrandModel[] = [];
  selectedBrand: number | null = null;

  productName: string = '';
  productDescription: string = '';
  productPrice: number | null = null;
  productStock: number | null = null;
  productBrandName: string = '';

  constructor(
    private authService: AuthService,
    private router: Router,
    private productService: ProductService,
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
    });
    this.productBrandService.getAllBrands().subscribe((data) => {
      this.productBrands = data;
    });
  }

  toggleAddProductForm() {
    this.showAddProductForm = !this.showAddProductForm;
  }

  toggleAddBrandForm() {
    this.showBrandNameForm = !this.showBrandNameForm;
  }

  onSubmitAddProduct(event: Event) {
    event.preventDefault();
    this.toggleAddProductForm();
  }

  onSubmitAddBrand(event: Event) {
    event.preventDefault();

    if(!this.productBrandName || this.productBrandName == "")
    {
      console.warn("Brand name is empty")
      return;
    }

    this.addNewBrand(this.productBrandName);
    this.toggleAddBrandForm();
  }

  addNewProduct(
    name: string,
    description: string,
    price: number,
    stock: number,
    productItemType: number,
    brandId: number
  ) {
    this.productService
      .createProductAsync(
        name,
        description,
        price,
        stock,
        productItemType,
        brandId
      )
      .subscribe((data) => {
        console.log('New Product Added!');
      });
  }

  addNewBrand(brandName: string) {
    this.productBrandService.addBrand(brandName).subscribe((data) => {
      console.log('Brand Added!');
    });
  }
}
