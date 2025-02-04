import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { emptyCartGuard } from '../../core/guards/empty-cart.guard';
import { orderCompleteGuard } from '../../core/guards/order-complete.guard';
import { CheckoutSuccessComponent } from './checkout-success/checkout-success.component';
import { CheckoutComponent } from './checkout.component';

export const routes: Routes = [
  // Checkout
  {
    path: '',
    component: CheckoutComponent,
    canActivate: [authGuard, emptyCartGuard],
  },
  {
    path: 'success',
    component: CheckoutSuccessComponent,
    canActivate: [authGuard, orderCompleteGuard],
  },
];
