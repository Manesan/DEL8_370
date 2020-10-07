import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadPropertyDocsComponent } from './upload-property-docs.component';

describe('UploadPropertyDocsComponent', () => {
  let component: UploadPropertyDocsComponent;
  let fixture: ComponentFixture<UploadPropertyDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UploadPropertyDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadPropertyDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
