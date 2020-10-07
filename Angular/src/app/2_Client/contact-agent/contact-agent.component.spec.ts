import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactAgentComponent } from './contact-agent.component';

describe('ContactAgentComponent', () => {
  let component: ContactAgentComponent;
  let fixture: ComponentFixture<ContactAgentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContactAgentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContactAgentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
