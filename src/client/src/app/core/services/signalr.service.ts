import { Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
import { Order } from '../../shared/models/orders.models';
@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  private hubUrl = environment.hubUrl;
  hubConnection?: HubConnection;
  orderSignal = signal<Order | null>(null);

  createHubConnection() {
    console.log({ createHubConnection: this.hubUrl });
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl, { withCredentials: true })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('hubConnection started'))
      .catch((error) => {
        console.warn('error on connecting to hub');
        console.error(error);
      });

    this.hubConnection.on('OrderCompleteNotification', (order: Order) => {
      console.log({ OrderCompleteNotification: order });
      this.orderSignal.set(order);
    });
  }

  stopHubConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch((error) => {
        console.warn('error on stopping to hub');
        console.error(error);
      });
    }
  }
}
