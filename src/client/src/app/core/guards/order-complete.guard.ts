import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { OrderService } from '../services/order.service';
import { SnackbarService } from '../services/snackbar.service';

export const orderCompleteGuard: CanActivateFn = (route, state) => {
  const orderService = inject(OrderService);

  if (orderService.orderComplete)
    return true;

  const router = inject(Router);
  const snack = inject(SnackbarService);

  snack.warn('Order has already been processed');
  router.navigateByUrl('/shop');

  return false;
};
