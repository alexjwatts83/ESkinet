import { inject, Injectable } from '@angular/core';
import {
  loadStripe,
  Stripe,
  StripeAddressElement,
  StripeAddressElementOptions,
  StripeElements,
} from '@stripe/stripe-js';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CartService } from './cart.service';
import { Cart } from '../../shared/models/cart';
import { firstValueFrom, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class StripeService {
  private stripePromise: Promise<Stripe | null>;
  private baseUrl = environment.apiUrl;
  private httpClient = inject(HttpClient);
  private cartService = inject(CartService);
  private elements?: StripeElements;
  private addressElememnt?: StripeAddressElement;

  constructor() {
    this.stripePromise = loadStripe(environment.stripePublicKey);
  }

  getStripeInstance() {
    return this.stripePromise;
  }

  private async initialiseElements() {
    if (!this.elements) {
      const stripe = await this.getStripeInstance();
      if (stripe) {
        const cart = await firstValueFrom(this.creatOrUpdatePaymentIntent());
        this.elements = stripe.elements({
          clientSecret: cart.clientSecret,
          appearance: { labels: 'floating' },
        });
      } else {
        throw new Error('Stripe has not been loaded');
      }
    }

    return this.elements;
  }

  async createAddressElement() {
    if (!this.addressElememnt) {
      const elements = await this.initialiseElements();
      if (elements) {
        const options: StripeAddressElementOptions = {
          mode: 'shipping',
        };
        this.addressElememnt = elements.create('address', options);
      } else {
        throw new Error('Elements Instance has not been loaded');
      }
    }
    return this.addressElememnt;
  }

  creatOrUpdatePaymentIntent() {
    const cart = this.cartService.cart();
    if (cart == null) throw new Error('Problem retrieving cart');

    return this.httpClient
      .post<Cart>(
        `${this.baseUrl}/payments/${cart.id}`,
        {},
        { withCredentials: true }
      )
      .pipe(
        tap((data) => {
          console.log({ afterPaymentIntent: data });
          console.log({ items: data.items });
          this.cartService.cart.set(data);
        })
      );
  }
}
