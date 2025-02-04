import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { OrderDetailsComponent } from './order-details/order-details.component';
import { OrderComponent } from './order.component';
export const routes: Routes = [
  {
    path: '',
    component: OrderComponent,
    canActivate: [authGuard],
  },
  {
    path: ':id',
    component: OrderDetailsComponent,
    canActivate: [authGuard],
  },
];
