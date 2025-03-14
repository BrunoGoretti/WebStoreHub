import { OrderItemModel } from '../models/orderItemModel';

export interface OrderModel {
  orderId: number;
  userId: number;
  orderDate: Date;
  totalAmount: number;
  status: string;
  orderItems: OrderItemModel[];
}
