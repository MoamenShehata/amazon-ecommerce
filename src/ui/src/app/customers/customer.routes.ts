import { Routes } from "@angular/router";
import { authGuard } from "../core/guards/auth-guard";
import { MyOrdersComponent } from "./components/my-orders/my-orders.component";
import { OrderDetailsComponent } from "./components/order-details/order-details.component";
import { CustomerProfileComponent } from "./components/customer-profile/customer-profile.component";
import { CustomerProfileLayoutComponent } from "./components/customer-profile-layout/customer-profile-layout.component";

export const CustomerRoutes: Routes = [
  {
    path: "",
    component: CustomerProfileLayoutComponent,
    children: [
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
    ]
  },

];
