import { TestBed } from '@angular/core/testing';

import { EcommServiceService } from './ecomm-service.service';

describe('EcommServiceService', () => {
  let service: EcommServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EcommServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
