import { Component, inject, OnInit, output } from '@angular/core';
import { CheckoutService } from '../../../core/services/checkout.service';
import { AsyncPipe, CurrencyPipe, JsonPipe, NgIf } from '@angular/common';
import { MatRadioModule } from '@angular/material/radio';
import { CartService } from '../../../core/services/cart.service';
import { tap } from 'rxjs';
import { DeliveryMethod } from '../../../shared/models/deliveryMethod';
@Component({
  selector: 'app-checkout-delivery',
  standalone: true,
  imports: [NgIf, AsyncPipe, JsonPipe, MatRadioModule, CurrencyPipe],
  templateUrl: './checkout-delivery.component.html',
  styleUrl: './checkout-delivery.component.scss',
})
export class CheckoutDeliveryComponent {
  checkoutService = inject(CheckoutService);
  cartService = inject(CartService);
  deliveryComplete = output<boolean>();

  deliveryMethods$ = this.checkoutService.getDeliveryMethods().pipe(
    tap(methods => {
      const deliveryMethodId = this.cartService.cart()?.deliveryMethodId;
      if (deliveryMethodId != null) {
        const method = methods.find(x => x.id == deliveryMethodId);
        if (method) {
          this.cartService.selectedDelivery.set(method);
          this.deliveryComplete.emit(true);
        }
      }
    })
  );

  updatDeliveryMethod(method: DeliveryMethod) {
    this.cartService.selectedDelivery.set(method);
    const cart = this.cartService.cart();
    if (cart) {
      cart.deliveryMethodId = method.id;
      this.cartService.setCart(cart);
      this.deliveryComplete.emit(true);
    }
  }
}
