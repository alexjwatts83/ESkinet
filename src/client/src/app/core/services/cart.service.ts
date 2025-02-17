import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Cart, CartItem } from '../../shared/models/cart';
import { Product } from '../../shared/models/products';
import { map, take, tap } from 'rxjs';
import { DeliveryMethod } from '../../shared/models/deliveryMethod';
@Injectable({
  providedIn: 'root',
})
export class CartService {
  private baseUrl = environment.apiUrl;
  private httpClient = inject(HttpClient);

  cart = signal<Cart | null>(null);

  itemCount = computed(() => {
    return this.cart()?.items.reduce((sum, item) => sum + item.quantity, 0);
  });

  totals = computed(() => {
    const cart = this.cart();
    if (!cart) return null;
    const items = cart.items;
    const delivery = this.selectedDelivery();
    const subTotal = items.reduce(
      (sum, item) => sum + item.quantity * item.price,
      0
    );
    const shipping = delivery ? delivery.price : 0;
    const discount = 0;
    return {
      subTotal,
      shipping,
      discount,
      total: subTotal + shipping - discount,
    };
  });

  selectedDelivery = signal<DeliveryMethod | null>(null);

  getCart(id: string) {
    return this.httpClient.get<Cart>(`${this.baseUrl}/cart/${id}`).pipe(
      tap((cart) => {
        console.log({ getCart: cart });
        this.cart.set(cart);
      })
    );
  }

  setCart(cart: Cart) {
    console.log({ setCart: cart });
    return this.httpClient
      .post<Cart>(`${this.baseUrl}/cart/`, { cart })
      .subscribe({
        next: (response: { id: string }) => {
          console.log({ setCartResponse: response.id, cart });
          setTimeout(() => this.cart.set(cart));
        },
      });
  }

  createCart(): Cart {
    const cart = new Cart();

    localStorage.setItem('cart_id', cart.id);

    return cart;
  }

  addItemToCart(item: CartItem | Product, quantity = 1) {
    console.log({ addItemToCart: { item, quantity } });
    const cart = { ...(this.cart() ?? this.createCart()) } as Cart;
    console.log({ original: cart });
    if (this.isProduct(item)) {
      console.log('is product');
      item = this.mapProductToCartItem(item);
    }
    console.log(item);
    cart.items = this.addOrUpdateItem(cart.items, item, quantity);
    this.setCart(cart);
  }

  removeItemFromCart(productId: string, quantity = 1) {
    console.log({ removeItemFromCart: { productId, quantity } });
    const cart = { ...(this.cart() ?? this.createCart()) } as Cart;
    console.log({ original: cart });
    if (!cart) return;
    const index = cart.items.findIndex((x) => x.productId === productId);

    if (index === -1) {
      return;
    }

    if (cart.items[index].quantity > quantity) {
      cart.items[index].quantity -= quantity;
    } else {
      cart.items.splice(index, 1);
    }
    if (cart.items.length === 0) {
      this.deleteCart();
    } else {
      this.setCart(cart);
    }
  }

  deleteCart() {
    const cart = this.cart();
    if (!cart) return;
    return this.httpClient.delete(`${this.baseUrl}/cart/${cart.id}`).subscribe({
      next: () => {
        localStorage.removeItem('cart_id');
        this.cart.set(null);
      },
    });
  }

  private addOrUpdateItem(
    items: CartItem[],
    item: CartItem,
    quantity: number
  ): CartItem[] {
    const index = items.findIndex((x) => x.productId === item.productId);
    if (index === -1) {
      item.quantity = quantity;
      items.push(item);
    } else {
      items[index].quantity += quantity;
    }
    return items;
  }

  private mapProductToCartItem(item: Product): CartItem {
    return {
      productId: item.id,
      productName: item.name,
      price: item.price,
      quantity: 0,
      pictureUrl: item.pictureUrl,
      brand: item.brand,
      type: item.brand,
    };
  }

  private isProduct(item: CartItem | Product): item is Product {
    return (item as Product).id !== undefined;
  }
}
