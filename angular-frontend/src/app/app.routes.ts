import { Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { LogInComponent } from './components/log-in/log-in.component'; // Import LogInComponent
import { RegistrationComponent } from './components/registration/registration.component';

export const routes: Routes = [
  { path: 'products', component: ProductListComponent },
  { path: 'login', component: LogInComponent }, // Add login route
  { path: '', redirectTo: '/products', pathMatch: 'full' }, // Default route
  { path: 'registration', component: RegistrationComponent },
];
