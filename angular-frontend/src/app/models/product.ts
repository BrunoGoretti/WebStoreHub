export interface Product {
  productId: number;
  name: string;
  description: string;
  price: number;
  brand: { brandId: number; brandName: string };
  stock: number;
  images: { imageUrl: string; mainPicture: number }[];
  imageUrl?: string;
}
