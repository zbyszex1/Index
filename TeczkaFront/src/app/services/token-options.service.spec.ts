import { TestBed } from '@angular/core/testing';

import { TokenOptionsService } from './token-options.service';

describe('TokenOptionsService', () => {
  let service: TokenOptionsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TokenOptionsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
