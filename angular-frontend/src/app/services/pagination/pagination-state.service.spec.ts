import { TestBed } from '@angular/core/testing';

import { PaginationStateService } from './pagination-state.service';

describe('PaginationStateService', () => {
  let service: PaginationStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PaginationStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
