import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountsService } from '../services/accounts.service';
import { map, of } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const accountsService = inject(AccountsService);
  const router = inject(Router);

  if (accountsService.currentUser()) {
    return of(true);
  }

  return accountsService.getAuthState().pipe(
    map((data) => {
      if (data.isAuthenticated) return true;

      router.navigate(['/account/login'], {
        queryParams: { returnUrl: state.url },
      });

      return false;
    })
  );
};
