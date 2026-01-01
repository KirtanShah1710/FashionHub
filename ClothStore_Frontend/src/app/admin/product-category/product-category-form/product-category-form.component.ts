import { Component } from '@angular/core';
import { ProductCategory } from '../../../shared/models/ProductCategory';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductCategorService } from '../../../shared/services/product-categor.service';

@Component({
  selector: 'app-product-category-form',
  imports: [ReactiveFormsModule],
  templateUrl: './product-category-form.component.html',
  styleUrl: './product-category-form.component.css'
})
export class ProductCategoryFormComponent {
  form!: FormGroup;
  isEditMode = false;
  categoryId!: number;

  constructor(
    private fb: FormBuilder,
    private service: ProductCategorService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.buildForm();

    this.categoryId = Number(this.route.snapshot.paramMap.get('id'));

    if (this.categoryId) {
      this.isEditMode = true;
      this.loadCategory();
    }
  }

  private buildForm(): void {
    this.form = this.fb.group({
      productCategoryId: [0],
      name: ['', Validators.required]
    });
  }

  private loadCategory(): void {
    this.service.getById(this.categoryId).subscribe({
      next: res => this.form.patchValue(res),
      error: () => this.toastr.error('Failed to load category')
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const data = this.form.value as ProductCategory;

    if (this.isEditMode) {
      this.service.update(this.categoryId, data).subscribe({
        next: () => {
          this.toastr.success('Category updated successfully');
          this.router.navigate(['/admin/product-categories']);
        },
        error: err => this.toastr.error(err.error || 'Update failed')
      });
    } else {
      this.service.create(data).subscribe({
        next: () => {
          this.toastr.success('Category created successfully');
          this.router.navigate(['/admin/product-categories']);
        },
        error: err => this.toastr.error(err.error || 'Create failed')
      });
    }
  }
}
