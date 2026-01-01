import { Routes } from '@angular/router';

export const routes: Routes = [
    {
      path: 'auth',
      loadChildren: () =>
        import('./auth/auth.routes').then(m => m.AUTH_ROUTES)
    },
    {
      path: 'admin',
      loadChildren: () =>
        import('./admin/admin.module').then(m => m.AdminModule)
    },
    {
      path: 'customer',
      loadChildren: () =>
        import('./customer/customer.module')
          .then(m => m.CustomerModule)
    }

];
