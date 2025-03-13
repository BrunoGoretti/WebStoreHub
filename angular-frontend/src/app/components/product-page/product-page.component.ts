import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../services/product/product.service';
import { Product } from '../../models/product-model';
import { CommonModule } from '@angular/common';
import { UserItemCartService } from '../../services/userItemCart/user-item-cart.Service ';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-product-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-page.component.html',
  styleUrls: ['./product-page.component.css'],
})
export class ProductPageComponent {
  product: Product | null = null;
  isModalOpen: boolean = false;
  selectedImageUrl: string = '';
  userId: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private cartService: UserItemCartService,
    private authService: AuthService ) {}

    ngOnInit(): void {
      this.authService.getUserId().subscribe((id) => {
        if (id) {
          this.userId = id;
          console.log('User ID retrieved:', this.userId);
        } else {
          console.warn('User is not logged in.');
        }
      });

      const productId = Number(this.route.snapshot.paramMap.get('id'));
      if (!isNaN(productId)) {
        this.productService.getProductById(productId).subscribe(
          (data) => {
            console.log("Product loaded:", data);
            this.product = data;
          },
          (error) => console.error('Error fetching product:', error)
        );
      }
    }

  // Open the modal with the clicked image
  openImage(imageUrl: string): void {
    this.selectedImageUrl = imageUrl;
    this.isModalOpen = true;
  }

  // Close the modal
  closeImage(): void {
    this.isModalOpen = false;
    this.selectedImageUrl = '';
  }

  addToCart(product: Product): void {
    if (!this.userId) {
      alert('Please log in to add items to the cart.');
      return;
    }

    this.cartService.addToCart(this.userId, product.productId, 1).subscribe(
      () => {
        alert(`${product.name} has been added to the cart!`);
      },
      (error) => {
        console.error('Error adding product to cart:', error);
        alert('Failed to add product to cart.');
      }
    );
  }
}
