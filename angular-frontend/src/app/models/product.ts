export interface Product {
  productId: number;
  name: string;
  description: string;
  price: number;
  discountedPrice?: number;
  brand: { brandId: number; brandName: string };
  stock: number;
  images: { imageUrl: string; mainPicture: number }[];
  imageUrl?: string;
}
