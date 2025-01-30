import { inject, Injectable } from '@angular/core';
import {
  ConfirmationToken,
  loadStripe,
  Stripe,
  StripeAddressElement,
  StripeAddressElementOptions,
  StripeElements,
  StripePaymentElement,
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
  private paymentElement?: StripePaymentElement;

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
        let outputColor = 'color:blue; font-size:20px;';
        console.log('%c creatOrUpdatePaymentIntent start', outputColor);
        const cart = await firstValueFrom(this.creatOrUpdatePaymentIntent());
        console.log('%c creatOrUpdatePaymentIntent end', outputColor);
        console.log({ initialiseElements: cart });
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

  async createPaymentElemment() {
    if (!this.paymentElement) {
      const elements = await this.initialiseElements();
      if (elements) {
        this.paymentElement = elements.create('payment');
      } else {
        throw new Error('Elements Instance has not been loaded');
      }
    }

    return this.paymentElement;
  }

  async createAddressElement() {
    if (!this.addressElememnt) {
      const elements = await this.initialiseElements();
      if (elements) {
        const user = this.accountsService.currentUser();
        let defaultValues: StripeAddressElementOptions['defaultValues'] = {
          name: user ? `${user.firstName} ${user.lastName}` : undefined,
          address:
            user && user.address
              ? {
                  line1: user.address.line1,
                  line2: user.address.line2,
                  country: user.address.country,
                  city: user.address.city,
                  postal_code: user.address.postalCode,
                  state: user.address.state,
                }
              : undefined,
        };
        // console.log({ defaultValues });
        const options: StripeAddressElementOptions = {
          mode: 'shipping',
          defaultValues,
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
    console.log({ creatOrUpdatePaymentIntent: cart });
    if (cart == null) throw new Error('Problem retrieving cart');

    return this.httpClient
      .post<Cart>(
        `${this.baseUrl}/payments/${cart.id}`,
        {},
        { withCredentials: true }
      )
      .pipe(
        tap((data) => {
          console.log({ creatOrUpdatePaymentIntentTap: data });
          // console.log({ items: data.items });

          if (!data.clientSecret)
            console.warn(
              'Cart Payment Intent was updated but had no client secret'
            );

          if (!data.deliveryMethodId)
            console.warn(
              'Cart Payment Intent was updated but had no deliveryMethodId'
            );
          let outputColor = 'color:blue; font-size:20px;';
          console.log(
            '%c creatOrUpdatePaymentIntent response returned and calling set cart',
            outputColor
          );
          this.cartService.setCart(data);
        })
      );
  }

  async createConfirmationToken() {
    const stripe = await this.getStripeInstance();
    if (!stripe) throw new Error('Stripe has not been loaded');

    const elements = await this.initialiseElements();
    const result = await elements.submit();

    if (result.error) throw new Error(result.error.message);

    return await stripe.createConfirmationToken({ elements });
  }

  async confirmPayment(confirmationToken: ConfirmationToken) {
    const stripe = await this.getStripeInstance();

    if (!stripe) throw new Error('Stripe has not been loaded');

    const elements = await this.initialiseElements();
    const result = await elements.submit();

    if (result.error) throw new Error(result.error.message);

    const clientSecret = this.cartService.cart()?.clientSecret;
    if (clientSecret) {
      let outputColor = 'color:blue; font-size:20px;';
      console.info('%c confirmPayment start',outputColor);
      const paymentResult = await stripe.confirmPayment({
        // elements: elements,
        clientSecret: clientSecret,
        confirmParams: {
          confirmation_token: confirmationToken.id,
          return_url: 'https://localhost:4200/',
        },
        redirect: 'if_required',
      });
      console.log({ PaymentIntentResult: paymentResult });
      console.info('%c confirmPayment end',outputColor);
      return paymentResult;
    }

    throw new Error('Cart was missing client secret');
  }

  disposeElements() {
    this.elements = undefined;
    this.addressElememnt = undefined;
    this.paymentElement = undefined;
  }
}
