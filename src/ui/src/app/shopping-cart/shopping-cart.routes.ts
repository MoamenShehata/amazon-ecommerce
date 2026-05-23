import {Routes} from "@angular/router";
import {ShoppingCartComponent} from "./components/shopping-cart/shopping-cart.component";
import {CartCheckoutComponent} from "./components/cart-checkout/cart-checkout.component";
import {authGuard} from "../core/guards/auth-guard";

export const shoppingCartRoutes: Routes = [
  {
    path: "",
    component: ShoppingCartComponent,
    children: [],
  },
  {
    path: "checkout",
    component: CartCheckoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: "cash",
        component: CartCheckoutComponent,
      },
      {
        path: "credit-card",
        component: CartCheckoutComponent,
      },
    ],
  },
];
