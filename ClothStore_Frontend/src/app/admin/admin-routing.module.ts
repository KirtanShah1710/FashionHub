import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { roleGuard } from '../core/guards/role.guard';

// Components
import { ProductCategoryListComponentComponent } from './product-category/product-category-list-component/product-category-list-component.component';
import { ProductCategoryFormComponent } from './product-category/product-category-form/product-category-form.component';

import { SizeFormComponent } from './size/size-form/size-form.component';
import { SizeListComponent } from './size/size-list/size-list.component';

import { ColorFormComponent } from './color/color-form/color-form.component';
import { ColorListComponent } from './color/color-list/color-list.component';

import { ProductCreateComponent } from './product/product-create/product-create.component';
import { ProductVariantFormComponent } from './product/product-variant-form/product-variant-form.component';

// Gender Components
import { GenderListComponent } from './gender/gender-list/gender-list.component';
import { GenderFormComponent } from './gender/gender-form/gender-form.component';

// Product component
import { ProductListComponent } from './product/product-list/product-list.component';
import { ProductEditComponent } from './product/product-edit/product-edit.component';
import { AdminOrderPageComponent } from './admin-order-page/admin-order-page.component';

const routes: Routes = [
  {
    path: '',
    canActivate: [roleGuard],
    data: { role: 'Admin' },
    children: [
      // Product Category Routes
      { path: 'product-categories', component: ProductCategoryListComponentComponent },
      { path: 'product-categories/create', component: ProductCategoryFormComponent },
      { path: 'product-categories/edit/:id', component: ProductCategoryFormComponent },

      // Size Routes
      { path: 'sizes', component: SizeListComponent, canActivate: [roleGuard], data: { role: 'Admin' } },
      { path: 'sizes/create', component: SizeFormComponent, canActivate: [roleGuard], data: { role: 'Admin' } },
      { path: 'sizes/edit/:id', component: SizeFormComponent, canActivate: [roleGuard], data: { role: 'Admin' } },

      // Color Routes
      { path: 'colors', component: ColorListComponent, canActivate: [roleGuard], data: { role: 'Admin' } },
      { path: 'colors/create', component: ColorFormComponent, canActivate: [roleGuard], data: { role: 'Admin' } },
      { path: 'colors/edit/:id', component: ColorFormComponent, canActivate: [roleGuard], data: { role: 'Admin' } },

      // Gender Routes
      { path: 'genders', component: GenderListComponent, canActivate: [roleGuard], data: { role: 'Admin' } },
      { path: 'genders/create', component: GenderFormComponent, canActivate: [roleGuard], data: { role: 'Admin' } },
      { path: 'genders/edit/:id', component: GenderFormComponent, canActivate: [roleGuard], data: { role: 'Admin' } },

      // Product Routes
      { path: 'products', component: ProductListComponent },
      { path: 'products/create', component: ProductCreateComponent },
      { path: 'products/edit/:id', component: ProductEditComponent },

      // Product Variant Route
      { path: 'products/variant/:productId', component: ProductVariantFormComponent },

      // order
      //{ path: 'all-order', component: AdminOrdersComponent }
      { path: 'all-orders', component: AdminOrderPageComponent}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
exports: [RouterModule]
})
export class AdminRoutingModule { }
