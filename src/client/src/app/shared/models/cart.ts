import { nanoid } from 'nanoid';

export type CarType = {
  id: string;
  items: CartItem[];
  deliveryMethodId?: string;
  clientSecret?: string
  paymentIntentId?: string;
};

export type CartItem = {
  productId: string;
  productName: string;
  price: number;
  pictureUrl: string;
  type: string;
  brand: string;
  quantity: number;
};

export class Cart implements CarType {
  id: string = nanoid();
  items: CartItem[] = [];
  deliveryMethodId?: string;
  clientSecret?: string
  paymentIntentId?: string;
}
