import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentPurchaseOffersComponent } from './agent-purchase-offers.component';

describe('AgentPurchaseOffersComponent', () => {
  let component: AgentPurchaseOffersComponent;
  let fixture: ComponentFixture<AgentPurchaseOffersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgentPurchaseOffersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentPurchaseOffersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
