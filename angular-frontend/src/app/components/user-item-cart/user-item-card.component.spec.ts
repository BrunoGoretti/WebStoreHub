import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserItemCartComponent } from './user-item-card.component';

describe('UserItemCardComponent', () => {
  let component: UserItemCartComponent;
  let fixture: ComponentFixture<UserItemCartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserItemCartComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserItemCartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
