export interface Product {
  productId: number;
  name: string;
  description: string;
  price: number;
  discountedPrice?: number;
  discountPercentage?: number;
  brand: { brandId: number; brandName: string };
  productType: { productTypeId: number; typeName: string };
  stock: number;
  images: { imageUrl: string; mainPicture: number }[];
  imageUrl?: string;
}
