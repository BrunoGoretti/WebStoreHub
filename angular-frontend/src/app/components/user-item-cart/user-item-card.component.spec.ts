import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserItemCardComponent } from './user-item-card.component';

describe('UserItemCardComponent', () => {
  let component: UserItemCardComponent;
  let fixture: ComponentFixture<UserItemCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserItemCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserItemCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
