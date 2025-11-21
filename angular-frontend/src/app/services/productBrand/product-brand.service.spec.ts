import { TestBed } from '@angular/core/testing';

import { ProductBrandService } from '../productBrand/product-brand.service';

describe('ProductBrandService', () => {
  let service: ProductBrandService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProductBrandService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
