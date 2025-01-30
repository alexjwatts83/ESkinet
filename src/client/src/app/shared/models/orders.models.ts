import { DeliveryMethod } from './deliveryMethod';

export type Order = {
  id: string;
  orderDate: string;
  buyerEmail: string;
  deliveryMethod: DeliveryMethod;
  shippingAddress: ShippingAddress;
  paymentSummary: PaymentSummary;
  status: string;
  subTotal: number;
  paymentIntentId: string;
  orderItems: OrderItem[];
  total: number;
};

export type ShippingAddress = {
  name: string;
  line1: string;
  line2?: string;
  city: string;
  state: string;
  postalCode: string;
  country: string;
};

export type PaymentSummary = {
  last4: number;
  brand: string;
  expMonth: number;
  expYear: number;
};

export type OrderItem = {
  id: string;
  orderId: string;
  itemOrdered: ItemOrdered;
  price: number;
  quantity: number;
};

export type ItemOrdered = {
  productId: string;
  productName: string;
  pictureUrl: string;
  type: string;
  brand: string;
};

export type CreateOrderDto = {
  cartId: string;
  deliveryMethodId: string;
  shippingAddress: ShippingAddress;
  paymentSummary: PaymentSummary;
};
