import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
@Injectable({
  providedIn: 'root',
})
export class SnackbarService {
  private snackbar = inject(MatSnackBar);
  private durationInSeconds  = 50;
  private milliseconds = 1000;

  error(message: string) {
    this.snackbar.open(message, 'Close', {
      duration: this.durationInSeconds * this.milliseconds,
      panelClass: ['snack-error'],
    });
  }

  success(message: string) {
    this.snackbar.open(message, 'Close', {
      duration: this.durationInSeconds * this.milliseconds,
      panelClass: ['snack-success'],
    });
  }
}
