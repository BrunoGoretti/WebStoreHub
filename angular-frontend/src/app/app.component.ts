import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from '../app/components/header/header.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, HeaderComponent], // Import HeaderComponent
  template: `
    <app-header></app-header>
    <router-outlet></router-outlet>
  `, // Add the header to the template
  styleUrls: ['./app.component.css'],
})
export class AppComponent {}
