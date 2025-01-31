import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { OrderSummaryComponent } from '../../shared/components/order-summary/order-summary.component';
import { MatStepper, MatStepperModule } from '@angular/material/stepper';
import { MatButton } from '@angular/material/button';
import { Router, RouterLink } from '@angular/router';
import { StripeService } from '../../core/services/stripe.service';
import {
  ConfirmationToken,
  StripeAddressElement,
  StripeAddressElementChangeEvent,
  StripePaymentElement,
  StripePaymentElementChangeEvent,
} from '@stripe/stripe-js';
import { SnackbarService } from '../../core/services/snackbar.service';
import { CartService } from '../../core/services/cart.service';
import { CurrencyPipe, JsonPipe } from '@angular/common';
import {
  MatCheckboxChange,
  MatCheckboxModule,
} from '@angular/material/checkbox';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { Address } from '../../shared/models/user';
import { CheckoutDeliveryComponent } from './checkout-delivery/checkout-delivery.component';
import { firstValueFrom } from 'rxjs';
import { CheckoutReviewComponent } from './checkout-review/checkout-review.component';
import { AccountsService } from '../../core/services/accounts.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import {
  CreateOrderDto,
  Order,
  ShippingAddress,
} from '../../shared/models/orders.models';
import { OrderService } from '../../core/services/order.service';
@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [
    OrderSummaryComponent,
    MatStepperModule,
    MatButton,
    RouterLink,
    JsonPipe,
    MatCheckboxModule,
    CheckoutDeliveryComponent,
    CheckoutReviewComponent,
    CurrencyPipe,
    MatProgressSpinnerModule,
  ],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss',
})
export class CheckoutComponent implements OnInit, OnDestroy {
  private stripeService = inject(StripeService);
  private snack = inject(SnackbarService);
  private router = inject(Router);
  private accountsService = inject(AccountsService);
  private orderService = inject(OrderService);

  cartService = inject(CartService);
  addressElement?: StripeAddressElement;
  saveAddress: boolean = false;
  shippingAddress = signal<Address | null>(null);
  private paymentElement?: StripePaymentElement;

  completionStatus = signal<{
    address: boolean;
    card: boolean;
    delivery: boolean;
  }>({
    address: false,
    card: false,
    delivery: false,
  });
  confirmationToken?: ConfirmationToken;
  loading = false;

  async ngOnInit() {
    try {
      this.addressElement = await this.stripeService.createAddressElement();
      this.addressElement.mount('#address-element');
      this.addressElement.on('change', this.handleAddressChange);

      this.paymentElement = await this.stripeService.createPaymentElemment();
      this.paymentElement.mount('#payment-element');
      this.paymentElement.on('change', this.handlePaymentChange);
    } catch (error: any) {
      this.snack.error(error.message);
    }
  }

  private handleAddressChange = (ev: StripeAddressElementChangeEvent) => {
    this.completionStatus.update((state) => {
      state.address = ev.complete;
      return state;
    });
  };

  private handlePaymentChange = (ev: StripePaymentElementChangeEvent) => {
    this.completionStatus.update((state) => {
      state.card = ev.complete;
      return state;
    });
  };

  async getConfirmationToken() {
    try {
      const allCompleted = Object.values(this.completionStatus()).every(
        (status) => status === true
      );
      console.log({ allCompleted });
      if (allCompleted) {
        const result = await this.stripeService.createConfirmationToken();
        if (result.error) throw new Error(result.error.message);
        console.log({ confirmationToken: result.confirmationToken });
        this.confirmationToken = result.confirmationToken;
      }
    } catch (error: any) {
      this.snack.error(error.message);
    }
  }

  handleDeliveryChange(ev: boolean) {
    this.completionStatus.update((state) => {
      state.delivery = ev;
      return state;
    });
  }

  ngOnDestroy(): void {
    this.stripeService.disposeElements();
  }

  onSaveAddressChanged($event: MatCheckboxChange) {
    this.saveAddress = $event.checked;
  }

  async onStepChanged($event: StepperSelectionEvent) {
    const selectedIndex = $event.selectedIndex;
    if (selectedIndex === 1) {
      const address = (await this.getAddressFromStripeAddress()) as Address;
      this.shippingAddress.set(address);
    }
    if (selectedIndex === 2) {
      if (this.cartService.selectedDelivery()) {
        await firstValueFrom(this.stripeService.creatOrUpdatePaymentIntent());
      }
    }
    if (selectedIndex === 3) {
      await this.getConfirmationToken();
    }
  }

  async confirmPayment(stepper: MatStepper) {
    try {
      if (!this.confirmationToken) {
        throw new Error('Payment could not be made');
      }
      await this.processConfirmPayment(this.confirmationToken);
    } catch (error: any) {
      console.log({ error });
      this.snack.error(error.message || 'Something went wrong during payment');
      stepper.previous();
    } finally {
      this.loading = false;
    }
  }

  private async processConfirmPayment(confirmationToken: ConfirmationToken) {
    let outputColor = 'color:red; font-size:20px;';
    console.info('%c stripeService.confirmPayment start', outputColor);
    this.loading = true;
    var response = await this.stripeService.confirmPayment(confirmationToken);
    console.info('%c stripeService.confirmPayment then', outputColor);
    // Handle successful result
    console.log({ confirmPaymentThen: response });

    // stripe error
    if (response.error) {
      throw new Error(response.error.message);
    }

    // I honestly dont know if this check is necessary
    if (response.paymentIntent?.status !== 'succeeded') {
      throw new Error(
        'Payment was not successful but was ' + response.paymentIntent?.status
      );
    }

    const order = await this.createOrderModel();
    const orderResult$ = this.orderService.creatOrUpdate(order);
    const orderResult = await firstValueFrom(orderResult$);
    console.log({ orderResult });
    if (!orderResult) {
      this.snack.error(
        'Payment was successful but there was an internal error'
      );
    }
    this.cartService.deleteCart();
    this.cartService.selectedDelivery.set(null);
    if (this.saveAddress) {
      const address = this.shippingAddress();
      if (address) {
        this.accountsService.addOrUpdateAddress(address).subscribe();
      }
    }
    this.router.navigateByUrl('/checkout/success');
  }

  private async getAddressFromStripeAddress(): Promise<
    Address | ShippingAddress | null
  > {
    const result = await this.addressElement?.getValue();
    const address = result?.value.address;
    if (address) {
      return {
        name: result.value.name,
        line1: address.line1,
        line2: address.line2 || undefined,
        city: address.city,
        state: address.state,
        country: address.country,
        postalCode: address.postal_code,
      };
    }
    return null;
  }

  private async createOrderModel() {
    const cart = this.cartService.cart();
    const shippingAddress =
      (await this.getAddressFromStripeAddress()) as ShippingAddress;
    const card = this.confirmationToken?.payment_method_preview.card;

    if (!cart?.id || !cart?.deliveryMethodId || !shippingAddress || !card)
      throw new Error('Problem Creating Order');

    return {
      cartId: cart.id,
      deliveryMethodId: cart.deliveryMethodId,
      paymentSummary: {
        last4: Number.parseInt(card.last4),
        brand: card.brand,
        expMonth: card.exp_month,
        expYear: card.exp_year,
      },
      shippingAddress,
    } as CreateOrderDto;
  }
}
