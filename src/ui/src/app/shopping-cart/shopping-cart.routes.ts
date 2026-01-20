import { Routes } from '@angular/router';
import { ShoppingCartComponent } from './components/shopping-cart/shopping-cart.component';

export const shoppingCartRoutes: Routes = [
  {
    path: '',
    component: ShoppingCartComponent,
    children: [],
  },
];
