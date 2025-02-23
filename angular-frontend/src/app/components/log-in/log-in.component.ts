import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-log-in',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css'],
})
export class LogInComponent {
  email: string = '';
  password: string = '';

  constructor(private router: Router, private authService: AuthService) {}

  onSubmit(): void {
    console.log('Submitting form with:', { email: this.email, password: this.password }); // Debugging
    this.authService.login(this.email, this.password).subscribe({
      next: (response) => {
        console.log('Login successful', response); // Debugging
        localStorage.setItem('token', response.token); // Store the token
        this.router.navigate(['/products']); // Redirect to products page
      },
      error: (error) => {
        console.error('Login failed', error); // Debugging
        alert('Login failed. Please check your credentials.'); // User feedback
      },
    });
  }

  redirectToRegistration(): void {
    this.router.navigate(['/registration']);
  }
}
