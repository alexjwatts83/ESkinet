import { Component, inject } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { OrderService } from '../../../core/services/order.service';
import { map, shareReplay, switchMap } from 'rxjs';
import { AsyncPipe, CurrencyPipe, DatePipe, JsonPipe, NgIf } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButton } from '@angular/material/button';
import { CardPipe } from '../../../shared/pipes/card.pipe';
import { AddressPipe } from '../../../shared/pipes/address.pipe';

@Component({
  selector: 'app-order-details',
  standalone: true,
  imports: [NgIf, AsyncPipe, JsonPipe, MatCardModule, MatButton, DatePipe, CurrencyPipe, AddressPipe, CardPipe],
  templateUrl: './order-details.component.html',
  styleUrl: './order-details.component.scss',
})
export class OrderDetailsComponent {
  private orderService = inject(OrderService);
  private activatedRoute = inject(ActivatedRoute);

  order$ = this.activatedRoute.params.pipe(
    map((p: Params) => p['id']),
    switchMap((id) => this.orderService.getForUserById(id)),
    shareReplay(1)
  );
}
