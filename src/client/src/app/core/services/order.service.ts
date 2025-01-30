import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Order, CreateOrderDto } from '../../shared/models/orders.models';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private baseUrl = environment.apiUrl;
  private httpClient = inject(HttpClient);
  private withCreds = {
    withCredentials: true,
  };

  getForUser() {
    return this.httpClient.get<Order[]>(
      `${this.baseUrl}/orders/`,
      this.withCreds
    );
  }

  getForUserById(id: string) {
    return this.httpClient.get<Order[]>(
      `${this.baseUrl}/orders/${id}`,
      this.withCreds
    );
  }

  creatOrUpdate(order: CreateOrderDto) {
    return this.httpClient.post<Order>(
      `${this.baseUrl}/orders`,
      { orderDto: order },
      this.withCreds
    );
  }
}
