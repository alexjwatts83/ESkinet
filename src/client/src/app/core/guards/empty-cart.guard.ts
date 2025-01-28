import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CartService } from '../services/cart.service';
import { SnackbarService } from '../services/snackbar.service';

export const emptyCartGuard: CanActivateFn = (route, state) => {
  const cartService = inject(CartService);

  if (cartService.cart() && cartService.cart()?.items.length) {
    return true;
  }

  const router = inject(Router);
  const snack = inject(SnackbarService);

  snack.error('You have no items in your cart');
  router.navigateByUrl('/cart');

  return true;
};
