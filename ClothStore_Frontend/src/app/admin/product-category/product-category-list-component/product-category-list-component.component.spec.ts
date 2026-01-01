import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductCategoryListComponentComponent } from './product-category-list-component.component';

describe('ProductCategoryListComponentComponent', () => {
  let component: ProductCategoryListComponentComponent;
  let fixture: ComponentFixture<ProductCategoryListComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductCategoryListComponentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductCategoryListComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
