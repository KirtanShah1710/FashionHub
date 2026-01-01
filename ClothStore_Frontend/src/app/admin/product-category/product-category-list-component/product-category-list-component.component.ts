import { Component } from '@angular/core';
import { ProductCategory } from '../../../shared/models/ProductCategory';
import { ToastrService } from 'ngx-toastr';
import { RouterModule } from '@angular/router';
import { ProductCategorService } from '../../../shared/services/product-categor.service';

@Component({
  selector: 'app-product-category-list-component',
  imports: [RouterModule],
  templateUrl: './product-category-list-component.component.html',
  styleUrl: './product-category-list-component.component.css'
})
export class ProductCategoryListComponentComponent {
  categories: ProductCategory[] = [];

  constructor(
    private service: ProductCategorService,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.loadCategories();
  }

  loadCategories() {
    this.service.getAll().subscribe({
      next: res => this.categories = res,
      error: () => this.toastr.error('Failed to load categories')
    });
  }

  delete(id: number) {
    if (!confirm('Delete this category?')) return;

    this.service.delete(id).subscribe({
      next: () => {
        this.toastr.success('Category deleted');
        this.loadCategories();
      },
      error: () => this.toastr.error('Delete failed')
    });
  }
}
