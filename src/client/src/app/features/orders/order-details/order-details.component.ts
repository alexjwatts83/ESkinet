import { Component, inject } from '@angular/core';
import { ActivatedRoute, Params, Router, RouterLink } from '@angular/router';
import { OrderService } from '../../../core/services/order.service';
import { map, shareReplay, switchMap } from 'rxjs';
import {
  AsyncPipe,
  CurrencyPipe,
  DatePipe,
  JsonPipe,
  NgIf,
} from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButton } from '@angular/material/button';
import { CardPipe } from '../../../shared/pipes/card.pipe';
import { AddressPipe } from '../../../shared/pipes/address.pipe';
import { AccountsService } from '../../../core/services/accounts.service';
import { AdminService } from '../../../core/services/admin.service';
import { TruncatePipe } from '../../../shared/pipes/truncate.pipe';

@Component({
  selector: 'app-order-details',
  standalone: true,
  imports: [
    NgIf,
    AsyncPipe,
    JsonPipe,
    MatCardModule,
    MatButton,
    DatePipe,
    CurrencyPipe,
    AddressPipe,
    CardPipe,
    TruncatePipe,
  ],
  templateUrl: './order-details.component.html',
  styleUrl: './order-details.component.scss',
})
export class OrderDetailsComponent {
  private orderService = inject(OrderService);
  private activatedRoute = inject(ActivatedRoute);
  private accountService = inject(AccountsService);
  private router = inject(Router);
  private adminService = inject(AdminService);

  buttonText = this.accountService.isAdmin()
    ? 'Return to Admin'
    : 'Return to Orders';
  order$ = this.activatedRoute.params.pipe(
    map((p: Params) => p['id']),
    switchMap((id) =>
      this.accountService.isAdmin()
        ? this.adminService.getOrder(id)
        : this.orderService.getForUserById(id)
    ),
    shareReplay(1)
  );

  onActionClick() {
    if (this.accountService.isAdmin()) {
      this.router.navigateByUrl('/admin');
      return;
    }
    this.router.navigateByUrl('/orders');
  }
}
