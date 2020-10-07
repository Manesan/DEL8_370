import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewClientRentalsComponent } from './view-client-rentals.component';

describe('ViewClientRentalsComponent', () => {
  let component: ViewClientRentalsComponent;
  let fixture: ComponentFixture<ViewClientRentalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewClientRentalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewClientRentalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
