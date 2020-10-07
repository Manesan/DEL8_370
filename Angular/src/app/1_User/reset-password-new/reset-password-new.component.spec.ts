import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetPasswordNewComponent } from './reset-password-new.component';

describe('ResetPasswordNewComponent', () => {
  let component: ResetPasswordNewComponent;
  let fixture: ComponentFixture<ResetPasswordNewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResetPasswordNewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResetPasswordNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
