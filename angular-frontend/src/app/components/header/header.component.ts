import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  username: string | null = '';
  isLoggedIn: boolean = false;

  constructor(public authService: AuthService) {}

  ngOnInit(): void {
    // Subscribe to authentication state changes
    this.authService.isLoggedIn().subscribe((loggedIn) => {
      this.isLoggedIn = loggedIn;
    });

    // Subscribe to username changes
    this.authService.getUsername().subscribe((username) => {
      this.username = username;
    });
  }

  logout(): void {
    this.authService.logout();
  }
}
