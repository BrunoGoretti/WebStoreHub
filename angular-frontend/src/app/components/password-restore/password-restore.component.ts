import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-password-restore',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './password-restore.component.html',
  styleUrl: './password-restore.component.css',
})
export class PasswordRestoreComponent {
  email: string = '';

  constructor(private authService: AuthService) {}

  onSubmit(): void {
    this.authService.passwordReset(this.email).subscribe({
      next: (response) => {
        console.log('Email sended!', response)
      },
      error: (error) => {
         console.error('Email failed', error);
      }
    })
  }
}
