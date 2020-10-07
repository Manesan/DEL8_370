import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MarketTypesComponent } from './market-types.component';

describe('MarketTypesComponent', () => {
  let component: MarketTypesComponent;
  let fixture: ComponentFixture<MarketTypesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MarketTypesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MarketTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
