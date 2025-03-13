import { TestBed } from '@angular/core/testing';

import { UserItemCartService } from './user-item-cart.Service ';

describe('CartServiceService', () => {
  let service: UserItemCartService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserItemCartService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
