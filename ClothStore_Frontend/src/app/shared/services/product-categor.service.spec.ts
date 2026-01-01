import { TestBed } from '@angular/core/testing';

import { ProductCategorService } from './product-categor.service';

describe('ProductCategorService', () => {
  let service: ProductCategorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProductCategorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
