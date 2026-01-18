import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () =>
      import('./authentication/authentication.routes').then(
        (m) => m.authRoutes,
      ),
  },
  {
    path: 'catalog',
    loadChildren: () =>
      import('./poduct-catalog/product-catalog.routes').then(
        (m) => m.catalogRoutes,
      ),
  },
];
