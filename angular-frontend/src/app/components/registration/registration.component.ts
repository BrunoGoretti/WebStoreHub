import { Component } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css'],
  standalone: true,
  imports: [FormsModule],
})
export class RegistrationComponent {
  username: string = '';
  fullname: string = '';
  email: string = '';
  password: string = '';

  constructor(private router: Router, private authService: AuthService) {}

  onRegister(): void {
    console.log('User registered:', { username: this.username, fullname: this.fullname, email: this.email, password: this.password });


    this.authService.Register(this.username, this.fullname, this.email, this.password).subscribe({
      next: (response) => {
        console.log('Registration successful', response)
        localStorage.setItem('token', response.token);
         alert('Registration successful!');
        this.router.navigate(['/products']);
      },
      error: (error) => {
        console.error('Registration failed', error);
        alert('Registration failed. Please check your credentials.');
      },
    })

  }
}
