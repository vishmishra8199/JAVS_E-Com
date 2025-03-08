import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VendorProfileComponent } from './vendor-profile.component';

describe('VendorProfileComponent', () => {
  let component: VendorProfileComponent;
  let fixture: ComponentFixture<VendorProfileComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VendorProfileComponent]
    });
    fixture = TestBed.createComponent(VendorProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
