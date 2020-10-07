import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RentalAgreementExtensionsComponent } from './rental-agreement-extensions.component';

describe('RentalAgreementExtensionsComponent', () => {
  let component: RentalAgreementExtensionsComponent;
  let fixture: ComponentFixture<RentalAgreementExtensionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RentalAgreementExtensionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RentalAgreementExtensionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
