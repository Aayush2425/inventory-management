import { Routes } from '@angular/router';
import { Login } from './features/auth/pages/login/login';
import { Register } from './features/auth/pages/register/register';
import { OwnerDashboard } from './features/dashboard/owner-dashboard/owner-dashboard';
import { Products } from './features/products/products';
import { AuthGuard } from './core/guards/auth-guard';
import { Categories } from './features/categories/categories';
import { Warehouses } from './features/warehouses/warehouses';
import { Suppliers } from './features/suppliers/suppliers';
import { Customers } from './features/customers/customers';
import { Purchaseorders } from './features/purchaseorders/purchaseorders';
import { Salesorders } from './features/salesorders/salesorders';

export const routes: Routes = [
  { path: '', redirectTo: '/Login', pathMatch: 'full' },
  { path: 'Login', component: Login, title: 'Login Page' },
  { path: 'Register', component: Register, title: 'Register Page' },
  {
    path: 'OwnerDashboard',
    component: OwnerDashboard,
    title: 'Owner DashBoard',
    canActivate: [AuthGuard],
  },
  { path: 'Products', component: Products, title: 'products', canActivate: [AuthGuard] },
  { path: 'Categories', component: Categories, title: 'categories', canActivate: [AuthGuard] },
  { path: 'Warehouses', component: Warehouses, title: 'warehouses', canActivate: [AuthGuard] },
  { path: 'Suppliers', component: Suppliers, title: 'suppliers', canActivate: [AuthGuard] },
  { path: 'Customers', component: Customers, title: 'customers', canActivate: [AuthGuard] },
  {
    path: 'PurchaseOrders',
    component: Purchaseorders,
    title: 'purchase orders',
    canActivate: [AuthGuard],
  },
  {
    path: 'SalesOrders',
    component: Salesorders,
    title: 'sales orders',
    canActivate: [AuthGuard],
  },
];
