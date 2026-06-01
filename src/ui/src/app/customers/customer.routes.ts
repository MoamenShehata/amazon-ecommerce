import { Routes } from "@angular/router";
import { authGuard } from "../core/guards/auth-guard";
import { MyOrdersComponent } from "./components/my-orders/my-orders.component";
import { OrderDetailsComponent } from "./components/order-details/order-details.component";
import { CustomerProfileComponent } from "./components/customer-profile/customer-profile.component";

export const CustomerRoutes: Routes = [
  {
    path: "profile",
    component: CustomerProfileComponent,
  },
  {
    path: "orders",
    component: MyOrdersComponent,
  },
  {
    path: "orders/:id",
    component: OrderDetailsComponent,
  },
];
