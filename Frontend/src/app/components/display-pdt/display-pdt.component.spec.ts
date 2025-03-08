import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DisplayPdtComponent } from './display-pdt.component';

describe('DisplayPdtComponent', () => {
  let component: DisplayPdtComponent;
  let fixture: ComponentFixture<DisplayPdtComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DisplayPdtComponent]
    });
    fixture = TestBed.createComponent(DisplayPdtComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
