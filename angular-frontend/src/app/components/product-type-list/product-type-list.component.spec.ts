import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductTypeListComponent } from './product-type-list.component';

describe('ProductTypeListComponent', () => {
  let component: ProductTypeListComponent;
  let fixture: ComponentFixture<ProductTypeListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductTypeListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductTypeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
