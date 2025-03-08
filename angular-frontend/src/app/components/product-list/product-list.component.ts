import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/product/product.service';
import { Product } from '../../models/product';
import { HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { PaginationComponent } from '../../components/pagination/pagination.component';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css', '../../components/pagination/pagination.component.css']
})
export class ProductListComponent extends PaginationComponent implements OnInit {

  constructor(private productService: ProductService, private router: Router) {
    super();
  }

  ngOnInit(): void {
    this.productService.getAllProducts().subscribe((data) => {
      this.products = data;
      this.updatePaginatedProducts();
    });
  }

  onProductClick(product: Product): void {
    this.router.navigate(['/product', product.productId]);
    console.log("Product clicked:", product);
  }
}
