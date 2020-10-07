import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BrowsePropertiesComponent } from './browse-properties.component';

describe('BrowsePropertiesComponent', () => {
  let component: BrowsePropertiesComponent;
  let fixture: ComponentFixture<BrowsePropertiesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BrowsePropertiesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BrowsePropertiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
