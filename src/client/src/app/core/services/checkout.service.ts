import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { DeliveryMethod } from '../../shared/models/deliveryMethod';
import { of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CheckoutService {
  private baseUrl = environment.apiUrl;
  private httpClient = inject(HttpClient);
  private deliveryMethods: DeliveryMethod[] = [];

  getDeliveryMethods() {
    if (this.deliveryMethods.length) return of(this.deliveryMethods);

    return this.httpClient
      .get<DeliveryMethod[]>(`${this.baseUrl}/payments/delivery-methods`)
      .pipe(
        tap((deliveryMethods) => {
          this.deliveryMethods = deliveryMethods;
        })
      );
  }
}
