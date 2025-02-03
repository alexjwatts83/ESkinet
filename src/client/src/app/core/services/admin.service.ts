import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { OrderParams } from '../../shared/models/shop-params';
import { Order } from '../../shared/models/orders.models';
import { Observable } from 'rxjs';
import { Pagination } from '../../shared/models/pagination';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private baseUrl = environment.apiUrl;
  private httpClient = inject(HttpClient);

  getOrders(orderParams: OrderParams): Observable<Pagination<Order>> {
    let params = new HttpParams();
    console.log({ orderParams });
    if (orderParams.filter && orderParams.filter !== 'All') {
      params = params.append('filter', orderParams.filter);
    }

    params = params.append('pageNumber', orderParams.pageNumber);
    params = params.append('pageSize', orderParams.pageSize);
    console.log({ params });

    return this.httpClient.get<Pagination<Order>>(
      `${this.baseUrl}/admin/orders`,
      { params, withCredentials: true }
    );
  }

  getOrder(id: string): Observable<Order> {
    console.log({ getOrder: id });
    return this.httpClient.get<Order>(`${this.baseUrl}/admin/orders/${id}`, {
      withCredentials: true,
    });
  }

  refundOrder(id: string): Observable<Order> {
    console.log({ refundOrder: id });
    return this.httpClient.post<Order>(
      `${this.baseUrl}/admin/orders/${id}/refund`,
      {},
      { withCredentials: true }
    );
  }
}
