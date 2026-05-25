import {Routes} from "@angular/router";
import {authGuard} from "../core/guards/auth-guard";
import {MyOrdersComponent} from "./components/my-orders/my-orders.component";
import {OrderDetailsComponent} from "./components/order-details/order-details.component";

export const CustomerRoutes: Routes = [
  {
    path: "orders",
    component: MyOrdersComponent,
  },
  {
    path: "orders/:id",
    component: OrderDetailsComponent,
  },
];
