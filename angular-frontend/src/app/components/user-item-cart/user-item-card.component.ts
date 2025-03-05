import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user-item-card',
  standalone: true,
  imports: [],
  templateUrl: './user-item-card.component.html',
  styleUrl: './user-item-card.component.css'
})
export class UserItemCartComponent {
  constructor(private route: ActivatedRoute) {

  }
}
