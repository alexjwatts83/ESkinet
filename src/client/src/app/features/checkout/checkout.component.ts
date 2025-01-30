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
  ],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss',
})
export class CheckoutComponent implements OnInit, OnDestroy {
  private stripeService = inject(StripeService);
  private snack = inject(SnackbarService);
  private router = inject(Router);
  private accountsService = inject(AccountsService);

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
      const address = await this.getAddressFromStripeAddress();
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
    // if (this.saveAddress) {
    //   const address = this.shippingAddress();
    //   if (address) {
    //     this.accountsService.addOrUpdateAddress(address).subscribe();
    //   }
    // }
    try {
      if (this.confirmationToken) {
        let outputColor = 'color:red; font-size:20px;';
        console.info('%c stripeService.confirmPayment start',outputColor);
        const result = await this.stripeService.confirmPayment(
          this.confirmationToken
        );
        console.info('%c stripeService.confirmPayment end',outputColor);
        if (result.error) {
          throw new Error(result.error.message);
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
    } catch (error: any) {
      this.snack.success(
        error.message || 'Something went wrong during payment'
      );
      stepper.previous();
    }
  }

  private async getAddressFromStripeAddress(): Promise<Address | null> {
    const result = await this.addressElement?.getValue();
    const address = result?.value.address;
    if (address) {
      return {
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
}
