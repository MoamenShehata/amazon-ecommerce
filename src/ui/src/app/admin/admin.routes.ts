import { Routes } from "@angular/router";
import { SystemOrdersComponent } from "./components/system-orders/system-orders.component";
import { AdminOrderDetailsComponent } from "./components/admin-order-details/admin-order-details.component";

export const AdminRoutes: Routes = [
  {
    path: "orders",
    component: SystemOrdersComponent,
  },
  {
    path: "orders/:id",
    component: AdminOrderDetailsComponent,
  },

];
