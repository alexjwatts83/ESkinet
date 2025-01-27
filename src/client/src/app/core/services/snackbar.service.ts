import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
@Injectable({
  providedIn: 'root',
})
export class SnackbarService {
  private snackbar = inject(MatSnackBar);
  private duration = 50000;

  error(message: string) {
    this.snackbar.open(message, 'Close', {
      duration: this.duration,
      panelClass: ['snack-error'],
    });
  }

  success(message: string) {
    this.snackbar.open(message, 'Close', {
      duration: this.duration,
      panelClass: ['snack-success'],
    });
  }
}
