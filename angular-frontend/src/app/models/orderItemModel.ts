import { Product } from '../models/product-model';


export interface OrderItemModel {
  orderItemId: number;
  orderId: number;
  productId: number;
  quantity: number;
  price: number;
  product: Product;
}
