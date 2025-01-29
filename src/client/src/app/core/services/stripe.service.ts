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
import { AccountsService } from './accounts.service';

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
  private accountsService = inject(AccountsService);

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
        const user = this.accountsService.currentUser();
        let defaultValues: StripeAddressElementOptions['defaultValues'] = {
          name: user ? `${user.firstName} ${user.lastName}` : undefined,
          address: user && user.address ? {
            line1: user.address.line1,
            line2: user.address.line2,
            country: user.address.country,
            city: user.address.city,
            postal_code: user.address.postalCode,
            state: user.address.state
          }
          : undefined
        };
        console.log({defaultValues});
        const options: StripeAddressElementOptions = {
          mode: 'shipping',
          defaultValues
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
          this.cartService.setCart(cart)
        })
      );
  }

  disposeElements() {
    this.elements = undefined;
    this.addressElememnt = undefined;
  }
}
