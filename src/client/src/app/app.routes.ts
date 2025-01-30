import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { ShopComponent } from './features/shop/shop.component';
import { ProductDetailsComponent } from './features/shop/product-details/product-details.component';
import { TestErrorComponent } from './features/test-error/test-error.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { ServerErrorComponent } from './shared/components/server-error/server-error.component';
import { CartComponent } from './features/cart/cart.component';
import { CheckoutComponent } from './features/checkout/checkout.component';
import { LoginComponent } from './features/accounts/login/login.component';
import { RegisterComponent } from './features/accounts/register/register.component';
import { authGuard } from './core/guards/auth.guard';
import { emptyCartGuard } from './core/guards/empty-cart.guard';
import { CheckoutSuccessComponent } from './features/checkout/checkout-success/checkout-success.component';
import { OrderComponent } from './features/orders/order.component';
import { OrderDetailsComponent } from './features/orders/order-details/order-details.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  // Shop
  { path: 'shop', component: ShopComponent },
  { path: 'shop/:id', component: ProductDetailsComponent },
  // Cart
  { path: 'cart', component: CartComponent },
  // Checkout
  {
    path: 'checkout',
    component: CheckoutComponent,
    canActivate: [authGuard, emptyCartGuard],
  },
  {
    path: 'checkout/success',
    component: CheckoutSuccessComponent,
    canActivate: [authGuard],
  },
  // Orders
  {
    path: 'orders',
    component: OrderComponent,
    canActivate: [authGuard],
  },
  {
    path: 'orders/:id',
    component: OrderDetailsComponent,
    canActivate: [authGuard],
  },
  // Accounts
  { path: 'account/login', component: LoginComponent },
  { path: 'account/register', component: RegisterComponent },
  // Others
  { path: 'test-error', component: TestErrorComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  // Redirect to not found
  { path: '**', redirectTo: 'not-found', pathMatch: 'full' },
];
