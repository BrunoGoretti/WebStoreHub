import { TestBed } from '@angular/core/testing';

import { ItemCartService } from './cart-item.service';

describe('CartServiceService', () => {
  let service: ItemCartService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ItemCartService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
