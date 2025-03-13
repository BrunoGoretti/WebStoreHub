import { Product } from '../models/product-model';

export interface CartItemModel {
  cartItemId: number;
  userId: number;
  productId: number;
  quantity: number;
  product: Product;
}
