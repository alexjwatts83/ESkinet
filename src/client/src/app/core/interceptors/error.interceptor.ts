import {
  HttpErrorResponse,
  HttpHeaderResponse,
  HttpInterceptorFn,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnackbarService } from '../services/snackbar.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const snackbar = inject(SnackbarService);

  return next(req).pipe(
    catchError((err: HttpErrorResponse | any) => {
      console.log(err);
      if (err.status == 400) {
        const message = err?.error?.detail || err?.error || err?.message;

        if (err?.error?.ValidationErrors) {
          console.log({ ValidationErrors: err.error.ValidationErrors });
          const validationErrors: string[] = [];
          for (const element of err?.error?.ValidationErrors) {
            // code
            console.log(element);
            validationErrors.push(element.errorMessage);
          }
          throw validationErrors.flat();
        }

        // alert(message);
        snackbar.error(message);
      }
      if (err.status == 401) {
        const message = err?.error?.title || err?.error || err?.message;
        // alert(message);
        snackbar.error(message);
      }
      if (err.status == 403) {
        snackbar.error('Forbidden');
      }
      if (err.status == 404) {
        router.navigateByUrl('/not-found');
      }
      if (err.status == 500) {
        const navigationExtras: NavigationExtras = {
          state: { error: err.error },
        };
        router.navigateByUrl('/server-error', navigationExtras);
      }
      return throwError(() => err);
    })
  );
};
