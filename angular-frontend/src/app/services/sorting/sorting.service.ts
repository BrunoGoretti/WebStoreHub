import { Injectable } from '@angular/core';
import { Product } from '../../models/product';

@Injectable({
  providedIn: 'root',
})
export class SortingService {
  sortProducts(products: Product[], sortOption: string): Product[] {
    switch (sortOption) {
      case 'priceLowToHigh':
        return [...products].sort((a, b) => a.price - b.price);
      case 'priceHighToLow':
        return [...products].sort((a, b) => b.price - a.price);
      case 'withbigdiscounts':
        return [...products].sort((a, b) => (b.discountPercentage || 0) - (a.discountPercentage || 0));
      default:
        return products;
    }
  }
}
