import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentSaleAgreementsComponent } from './agent-sale-agreements.component';

describe('AgentSaleAgreementsComponent', () => {
  let component: AgentSaleAgreementsComponent;
  let fixture: ComponentFixture<AgentSaleAgreementsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgentSaleAgreementsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentSaleAgreementsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
