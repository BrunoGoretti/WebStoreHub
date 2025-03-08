import { Component, OnInit  } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../services/product/product.service';
import { Product } from '../../models/product';
import { CommonModule } from '@angular/common';
import { PaginationComponent } from '../../components/pagination/pagination.component';

@Component({
  selector: 'app-search-product',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './search-product.component.html',
  styleUrls: ['./search-product.component.css', '../../components/pagination/pagination.component.css']
})

export class SearchProductComponent extends PaginationComponent implements OnInit {
  searchQuery: string = '';

  constructor(private route: ActivatedRoute, private productService: ProductService, private router: Router) {
    super();
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.searchQuery = params['q'];
      if (this.searchQuery) {
        this.productService.searchProductsByName(this.searchQuery).subscribe(
          (data) => {
            this.products = data;
            this.updatePaginatedProducts();
          },
          (error) => console.error('Error fetching search results:', error)
        );
      }
    });
  }

  onProductClick(product: Product): void {
    this.router.navigate(['/product', product.productId]);
  }
}
