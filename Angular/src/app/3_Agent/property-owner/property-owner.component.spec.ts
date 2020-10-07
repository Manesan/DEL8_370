import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PropertyOwnerComponent } from './property-owner.component';

describe('PropertyOwnerComponent', () => {
  let component: PropertyOwnerComponent;
  let fixture: ComponentFixture<PropertyOwnerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PropertyOwnerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PropertyOwnerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
