import { Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { CatalogLandingComponent } from './components/catalog-landing/catalog-landing.component';
import { ProductCreateComponent } from './components/product-create/product-create.component';

export const catalogRoutes: Routes = [
  {
    path: '',
    component: CatalogLandingComponent,
    children: [
      {
        path: 'products',
        component: ProductListComponent,
      },
      {
        path: 'products/create',
        component: ProductCreateComponent,
      },
    ],
  },
];
