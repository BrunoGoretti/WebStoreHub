import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-filter-sort',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './filter-sort.component.html',
  styleUrls: ['./filter-sort.component.css'],
})
export class FilterSortComponent {
  @Output() sortChange = new EventEmitter<string>();

  sortingOptions = [
    { value: '', label: 'Default' },
    { value: 'priceLowToHigh', label: 'Price Low To High' },
    { value: 'priceHighToLow', label: 'Price High To Low' },
    { value: 'discountHighToLow', label: 'Discount High To Low' },
  ];

  onSortChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const selectedOption = selectElement.value;
    this.sortChange.emit(selectedOption);
  }
}
