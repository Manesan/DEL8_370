import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValuationReportComponent } from './valuation-report.component';

describe('ValuationReportComponent', () => {
  let component: ValuationReportComponent;
  let fixture: ComponentFixture<ValuationReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValuationReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValuationReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
