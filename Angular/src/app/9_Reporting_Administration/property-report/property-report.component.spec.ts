import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PropertyReportComponent } from './property-report.component';

describe('PropertyReportComponent', () => {
  let component: PropertyReportComponent;
  let fixture: ComponentFixture<PropertyReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PropertyReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PropertyReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
