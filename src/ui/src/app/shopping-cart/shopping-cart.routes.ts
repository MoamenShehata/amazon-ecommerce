import {Routes} from "@angular/router";
import {ShoppingCartComponent} from "./components/shopping-cart/shopping-cart.component";
import {CartCheckoutComponent} from "./components/cart-checkout/cart-checkout.component";
import {authGuard} from "../core/guards/auth-guard";
import {CachCheckoutComponent} from "./components/cach-checkout/cach-checkout.component";

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
  },
  {
    path: "checkout/cash",
    component: CachCheckoutComponent,
  },
  {
    path: "checkout/credit-card",
    component: CartCheckoutComponent,
  },
];
