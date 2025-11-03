import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [NgIf, FormsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css',
})
export class AdminDashboardComponent implements OnInit {
  userId: number | null = null;
  showAddProductForm = false;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.authService.isLoggedIn().subscribe((islogged) => {
      if (!islogged) {
        console.warn('User not logged in.');
        this.router.navigate(['/login']);
      }
    });
  }

  toggleAddProductForm() {
    this.showAddProductForm = !this.showAddProductForm;
  }

  onSubmitAddProduct(event: Event) {
    event.preventDefault();
    this.toggleAddProductForm();
  }
}
