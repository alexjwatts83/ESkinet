import { HttpErrorResponse, HttpHeaderResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnackbarService } from '../services/snackbar.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const snackbar = inject(SnackbarService);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      console.log(err);
      if (err.status == 400) {
        const message = err?.error?.title || err?.error || err?.message;
        // alert(message);
        snackbar.error(message);
      }
      if (err.status == 401) {
        const message = err?.error?.title || err?.error || err?.message;
        // alert(message);
        snackbar.error(message);
      }
      if (err.status == 404) {
        router.navigateByUrl('/not-found');
      }
      if (err.status == 500) {
        router.navigateByUrl('/server-error');
      }
      return throwError(() => err);
    })
  );
};
