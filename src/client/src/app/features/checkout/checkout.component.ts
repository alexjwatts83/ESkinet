import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { OrderSummaryComponent } from '../../shared/components/order-summary/order-summary.component';
import { MatStepperModule } from '@angular/material/stepper';
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { StripeService } from '../../core/services/stripe.service';
import { StripeAddressElement, StripePaymentElement } from '@stripe/stripe-js';
import { SnackbarService } from '../../core/services/snackbar.service';
import { CartService } from '../../core/services/cart.service';
import { JsonPipe, NgIf } from '@angular/common';
import {
  MatCheckboxChange,
  MatCheckboxModule,
} from '@angular/material/checkbox';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { Address } from '../../shared/models/user';
import { CheckoutDeliveryComponent } from "./checkout-delivery/checkout-delivery.component";
import { firstValueFrom } from 'rxjs';
@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [
    OrderSummaryComponent,
    MatStepperModule,
    MatButton,
    RouterLink,
    JsonPipe,
    NgIf,
    MatCheckboxModule,
    CheckoutDeliveryComponent
],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss',
})
export class CheckoutComponent implements OnInit, OnDestroy {
  private stripeService = inject(StripeService);
  private snack = inject(SnackbarService);

  cartService = inject(CartService);
  addressElement?: StripeAddressElement;
  saveAddress: boolean = false;
  shippingAddress = signal<Address | null>(null);
  private paymentElement?: StripePaymentElement;
  async ngOnInit() {
    try {
      this.addressElement = await this.stripeService.createAddressElement();
      this.addressElement.mount('#address-element');
      this.paymentElement = await this.stripeService.createPaymentElemment();
      this.paymentElement.mount('#payment-element');
    } catch (error: any) {
      this.snack.error(error.message);
    }
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
      if (this.saveAddress) {
        const address = await this.getAddressFromStripeAddress();
        this.shippingAddress.set(address);
      }
    }
    if (selectedIndex === 2) {
      if (this.cartService.selectedDelivery()) {
        await firstValueFrom(this.stripeService.creatOrUpdatePaymentIntent());
      }
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
