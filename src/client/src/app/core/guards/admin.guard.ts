import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountsService } from '../services/accounts.service';
import { SnackbarService } from '../services/snackbar.service';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountsService = inject(AccountsService);
  const router = inject(Router);
  const snack = inject(SnackbarService);

  if (accountsService.isAdmin()){
    return true;
  }

  snack.error('Not an admin');
  router.navigateByUrl('/shop')
  return false;
};
