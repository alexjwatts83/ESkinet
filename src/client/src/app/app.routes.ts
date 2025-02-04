import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { ShopComponent } from './features/shop/shop.component';
import { ProductDetailsComponent } from './features/shop/product-details/product-details.component';
import { TestErrorComponent } from './features/test-error/test-error.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { ServerErrorComponent } from './shared/components/server-error/server-error.component';
import { CartComponent } from './features/cart/cart.component';
import { authGuard } from './core/guards/auth.guard';
import { AdminComponent } from './features/admin/admin.component';
import { adminGuard } from './core/guards/admin.guard';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  // Shop
  { path: 'shop', component: ShopComponent },
  { path: 'shop/:id', component: ProductDetailsComponent },
  // Cart
  { path: 'cart', component: CartComponent },
  // checkout
  {
    path: 'checkout',
    loadChildren: () =>
      import('./features/checkout/checkout.routes').then((r) => r.routes),
  },
  // Orders
  {
    path: 'orders',
    loadChildren: () =>
      import('./features/orders/orders.routes').then((r) => r.routes),
  },
  // Accounts
  {
    path: 'account',
    loadChildren: () =>
      import('./features/accounts/accounts.routes').then((r) => r.routes),
  },
  // Others
  { path: 'test-error', component: TestErrorComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  // admin
  {
    path: 'admin',
    loadComponent: () =>
      import('./features/admin/admin.component').then((c) => c.AdminComponent),
    canActivate: [authGuard, adminGuard],
  },
  // Redirect to not found
  { path: '**', redirectTo: 'not-found', pathMatch: 'full' },
];
