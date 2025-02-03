import { inject, Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from '../../shared/components/confirmation-dialog/confirmation-dialog.component';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DialogService {
  private dialg = inject(MatDialog);

  confirm(title: string, message: string) {
    const dialogRef = this.dialg.open(ConfirmationDialogComponent, {
      width: '400px',
      data: { title, message },
    });

    return firstValueFrom(dialogRef.afterClosed());
  }
}
