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
import { ImageUploadService } from '../../services/imageUpload/imageImgbb.service';

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
  showRemoveProductForm = false;
  showBrandNameForm = false;
  showBrandRemoveForm = false;

  productTypes: ProductTypeModel[] = [];
  selectedType: number | null = null;

  productBrands: ProductBrandModel[] = [];
  selectedBrand: number | null = null;

  selectedProduct: number | null = null;

  producdId: number | null = null;
  producdimageUrl: File | null = null;
  productName: string = '';
  productDescription: string = '';
  productPrice: number | null = null;
  productStock: number | null = null;
  productBrandName: string = '';
  mainImage: number | null = null;
  uploadedImage: string = '';

  constructor(
    private authService: AuthService,
    private router: Router,
    private productService: ProductService,
    private productTypeService: ProductTypeService,
    private productBrandService: ProductBrandService,
    private imageUploadService: ImageUploadService
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

  toggleRemoveProductForm() {
    this.showRemoveProductForm = !this.showRemoveProductForm;
  }

  toggleAddBrandForm() {
    this.showBrandNameForm = !this.showBrandNameForm;
  }

  toggleRemoveBrandForm() {
    this.showBrandRemoveForm = !this.showBrandRemoveForm;
  }

  onSubmitAddProduct(event: Event) {
    event.preventDefault();

    if (this.productPrice == null) {
      console.error('Price is empty');
      return;
    } else if (this.productStock == null) {
      console.error('Stock is empty');
      return;
    } else if (this.selectedType == null) {
      console.error('Type is empty');
      return;
    } else if (this.selectedBrand == null) {
      console.error('Brand is empty');
      return;
    }

    this.addNewProduct(
      this.productName,
      this.productDescription,
      this.productPrice,
      this.productStock,
      this.selectedType,
      this.selectedBrand
    );
    this.toggleAddProductForm();
  }

  onSubmitRemoveProduct(event: Event) {
    event.preventDefault();

    if (!this.selectedProduct || this.selectedProduct == null) {
      console.warn('Brand name is empty');
      return;
    }

    this.removeProduct(this.selectedProduct);
    this.toggleRemoveProductForm();
  }

  onSubmitAddBrand(event: Event) {
    event.preventDefault();

    if (!this.productBrandName || this.productBrandName == '') {
      console.warn('Brand name is empty');
      return;
    }

    this.addNewBrand(this.productBrandName);
    this.toggleAddBrandForm();
  }

  onSubmitRemoveBrand(event: Event) {
    event.preventDefault();

    if (!this.selectedBrand || this.selectedBrand == null) {
      console.warn('Brand ID is empty');
      return;
    }
    this.removeBrand(this.selectedBrand);
    this.toggleRemoveBrandForm();
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
      .subscribe((createdProduct) => {
        console.log('New Product Added!');
        if (!createdProduct || !createdProduct.productId) {
          console.error('Created product ID is missing.');
          return;
        }

        this.producdId = createdProduct.productId;

        if (!this.producdimageUrl) {
          console.warn('Main image file is not selected.');
          return;
        }
        if (this.mainImage === null) {
          console.warn('Main image flag not set.');
          return;
        }
        if (!this.producdimageUrl) {
          console.warn('Main image file is not selected.');
          return;
        }
        if (this.mainImage === null) {
          console.warn('Main image flag not set.');
          return;
        }
        if (!this.producdId) {
          console.log('Product Id is empty');
          return;
        }

        this.addProductPicture(
          this.producdId,
          this.producdimageUrl,
          this.mainImage
        );
      });
  }

  onMainImageSelected(event: any) {
    const file = event.target.files[0];

    if (!file) {
      return;
    }

    this.producdimageUrl = file;

    this.mainImage = 1;

    const reader = new FileReader();
    reader.onload = () => {
      this.uploadedImage = reader.result as string;
      console.log('Main image loaded:', this.uploadedImage);
    };
    reader.readAsDataURL(file);
  }

  onAdditionalImageSelected(event: any) {
    const file = event.target.files[0]

    if(!file) {
      return;
    }
    this.producdimageUrl = file;
    this.mainImage = 0;

  }

  removeProduct(productId: number) {
    this.productService.removeProduct(productId).subscribe((data) => {
      console.log('Product Removed!');
    });
  }

  addProductPicture(producdId: number, imageUrl: File, mainImage: number) {
    this.imageUploadService
      .addImage(producdId, imageUrl, mainImage)
      .subscribe((data) => {
        console.log('Picture Added!');
      });
  }

  addNewBrand(brandName: string) {
    this.productBrandService.addBrand(brandName).subscribe((data) => {
      console.log('Brand Added!');
    });
  }

  removeBrand(brandId: number) {
    this.productBrandService.revomeBrand(brandId).subscribe((data) => {
      console.log('Brand Removed!');
    });
  }
}
