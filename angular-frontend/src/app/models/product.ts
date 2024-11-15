export interface Product {
  productId: number;
  name: string;
  description: string;
  price: number;
  stock: number;
  images: { imageUrl: string; mainPicture: number }[]; // Include images array
  imageUrl?: string;  // Add this line for the imageUrl
}
