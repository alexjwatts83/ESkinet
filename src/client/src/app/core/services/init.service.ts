import { inject, Injectable } from '@angular/core';
import { CartService } from './cart.service';
import { forkJoin, of, tap } from 'rxjs';
import { AccountsService } from './accounts.service';
import { SignalrService } from './signalr.service';

@Injectable({
  providedIn: 'root'
})
export class InitService {
  private cartService = inject(CartService);
  private accountsService = inject(AccountsService);
  private signalrService = inject(SignalrService)
  
  init() {
    const cartId = localStorage.getItem('cart_id');
    const cart$ = cartId ? this.cartService.getCart(cartId) : of(null);
    
    return forkJoin( {
      cart: cart$,
      user: this.accountsService.getUserInfo().pipe(
        tap(user => {
          console.log({getUserInfoInit: user})
          if (user) {
            console.log('connect to signalr on init');
            this.signalrService.createHubConnection()
          }
        })
      )
    });
  }
}
