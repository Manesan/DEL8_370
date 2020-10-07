import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewClientPurchasesComponent } from './view-client-purchases.component';

describe('ViewClientPurchasesComponent', () => {
  let component: ViewClientPurchasesComponent;
  let fixture: ComponentFixture<ViewClientPurchasesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewClientPurchasesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewClientPurchasesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
