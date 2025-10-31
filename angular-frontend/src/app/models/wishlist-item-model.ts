import { Product } from './product-model';

export interface WishlistItemModel {
  WishlistItemId: number;
  userId: number;
  productId: number;
  product: Product;
}
