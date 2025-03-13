import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductPageComponent } from './components/product-page/product-page.component';
import { LogInComponent } from './components/log-in/log-in.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { UserItemCartComponent } from './components/user-item-cart/user-item-card.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SearchProductComponent } from './components/search-product/search-product.component';
import { ProductTypeListComponent } from './components/product-type-list/product-type-list.component';

export const routes: Routes = [
  { path: 'products', component: ProductListComponent },
  { path: 'product/:id', component: ProductPageComponent },
  { path: 'useritemcart', component: UserItemCartComponent },
  { path: 'login', component: LogInComponent },
  { path: '', redirectTo: '/products', pathMatch: 'full' },
  { path: 'registration', component: RegistrationComponent },
  { path: 'search', component: SearchProductComponent },
  { path: 'category/:typeName', component: ProductTypeListComponent },
  { path: 'product/:id', component: ProductPageComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
