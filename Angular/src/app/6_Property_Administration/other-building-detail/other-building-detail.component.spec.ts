import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OtherBuildingDetailComponent } from './other-building-detail.component';

describe('OtherBuildingDetailComponent', () => {
  let component: OtherBuildingDetailComponent;
  let fixture: ComponentFixture<OtherBuildingDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OtherBuildingDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OtherBuildingDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
