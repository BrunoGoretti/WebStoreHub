import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-reset-password-component',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './reset-password-component.component.html',
  styleUrl: './reset-password-component.component.css',
})
export class ResetPasswordComponent {
  token: string | null = null;
  newPassword: string = '';
  confirmPassword: string = '';
  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.token = this.route.snapshot.queryParamMap.get('token');
  }

  onSubmit() {
    if (!this.token) {
      this.errorMessage = 'Invalid password reset link.';
      return;
    }

    if (this.newPassword !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match.';
      return;
    }

    this.authService.resetPassword(this.token, this.newPassword).subscribe({
      next: () => {
        this.successMessage =
          'Password reset successful! Redirecting to login...';
        setTimeout(() => this.router.navigate(['/login']), 3000);
      },
      error: (error) => {
        if (error.error && typeof error.error === 'string') {
          this.errorMessage = 'Failed to reset password. ' + error.error;
        } else if (error.error && error.error.message) {
          this.errorMessage =
            'Failed to reset password. ' + error.error.message;
        } else if (error.message) {
          this.errorMessage = 'Failed to reset password. ' + error.message;
        } else {
          this.errorMessage =
            'Failed to reset password. An unknown error occurred.';
        }
      },
    });
  }
}
