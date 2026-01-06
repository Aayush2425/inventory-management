import { TestBed } from '@angular/core/testing';

import { OwnerServices } from './owner-services';

describe('OwnerServices', () => {
  let service: OwnerServices;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OwnerServices);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
