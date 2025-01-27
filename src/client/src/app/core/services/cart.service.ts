import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Cart, CartItem } from '../../shared/models/cart';
import { Product } from '../../shared/models/products';
import { map, tap } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class CartService {
  private baseUrl = environment.apiUrl;
  private httpClient = inject(HttpClient);

  cart = signal<Cart | null>(null);

  getCart(id: string) {
    return this.httpClient
      .get<Cart>(`${this.baseUrl}/cart/${id}`)
      .pipe(tap((cart) => this.cart.set(cart)));
  }

  setCart(cart: Cart) {
    return this.httpClient
      .post<Cart>(`${this.baseUrl}/cart/`, { cart })
      .subscribe({
        next: (response: { id: string }) => {
          console.log({ cartFromApi: response.id });
          // this.cart.set(cart);
          this.getCart(response.id);
        },
      });
  }

  createCart(): Cart {
    const cart = new Cart();

    localStorage.setItem('cart_id', cart.id);

    return cart;
  }

  addItemToCart(item: CartItem | Product, quantity = 1) {
    const cart = this.cart() ?? this.createCart();

    console.log({ cart });
    if (this.isProduct(item)) {
      console.log('is product');
      item = this.mapProductToCartItem(item);
    }
    console.log(item);
    cart.items = this.addOrUpdateItem(cart.items, item, quantity);
    this.setCart(cart);
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
