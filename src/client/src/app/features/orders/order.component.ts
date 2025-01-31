import { Component, inject } from '@angular/core';
import { OrderService } from '../../core/services/order.service';
import { AsyncPipe, CurrencyPipe, DatePipe, JsonPipe, NgFor, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [AsyncPipe, NgIf, NgFor, JsonPipe, CurrencyPipe, RouterLink, DatePipe],
  templateUrl: './order.component.html',
  styleUrl: './order.component.scss'
})
export class OrderComponent {
  private orderService = inject(OrderService);
  orders$ = this.orderService.getForUser();
}
