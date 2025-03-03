import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../services/product/product.service';
import { Product } from '../../models/product';
import { CommonModule } from '@angular/common';

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

  constructor(private route: ActivatedRoute, private productService: ProductService) {}

  ngOnInit(): void {
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
}
