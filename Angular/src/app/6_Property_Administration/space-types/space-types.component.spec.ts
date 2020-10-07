import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpaceTypesComponent } from './space-types.component';

describe('SpaceTypesComponent', () => {
  let component: SpaceTypesComponent;
  let fixture: ComponentFixture<SpaceTypesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpaceTypesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpaceTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
