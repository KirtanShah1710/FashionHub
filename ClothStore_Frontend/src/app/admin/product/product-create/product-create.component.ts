import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ProductService } from '../../../shared/services/product.service';
import { GenderService } from '../../../shared/services/gender.service';
import { ProductVariantFormComponent } from '../product-variant-form/product-variant-form.component';
import { ProductCategorService } from '../../../shared/services/product-categor.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product-create',
  imports: [ReactiveFormsModule, ProductVariantFormComponent],
  templateUrl: './product-create.component.html',
  styleUrl: './product-create.component.css'
})
export class ProductCreateComponent {
  productForm!: FormGroup;
  productImagePreview: string | null = null;
  selectedImageFile!: File;

  isSubmitting = false;
  createdProductId: number | null = null;

  productCategories: any[] = [];
  genderCategories: any[] = [];

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder,
    private productService: ProductService,
    private productCategoryService: ProductCategorService,
    private genderService: GenderService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadDropdowns();
  }

  private initForm() {
    this.productForm = this.fb.group({
      productName: ['', Validators.required],
      description: [''],
      productCategoryId: [null, Validators.required],
      genderCategoryId: [null, Validators.required],
      image: [null]
    });
  }

  private loadDropdowns() {
    // Load Product Categories
    this.productCategoryService.getAll().subscribe({
      next: res => this.productCategories = res,
      error: () => this.toastr.error('Failed to load product categories')
    });

    // Load Gender Categories
    this.genderService.getAll().subscribe({
      next: res => this.genderCategories = res,
      error: () => this.toastr.error('Failed to load gender categories')
    });
  }

  // ---------------- IMAGE PREVIEW ----------------
  onImageChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedImageFile = input.files[0];

      const reader = new FileReader();
      reader.onload = () => {
        this.productImagePreview = reader.result as string;
      };
      reader.readAsDataURL(this.selectedImageFile);
    }
  }

  // ---------------- SUBMIT ----------------
  submit() {
    if (this.productForm.invalid) return;

    this.isSubmitting = true;

    const formData = new FormData();
    formData.append('productName', this.productForm.value.productName);
    formData.append('description', this.productForm.value.description);
    formData.append('productCategoryId', this.productForm.value.productCategoryId);
    formData.append('genderCategoryId', this.productForm.value.genderCategoryId);

    if (this.selectedImageFile) {
      formData.append('image', this.selectedImageFile);
    }

    this.productService.create(formData).subscribe({
      next: (res: any) => {
        this.createdProductId = res.productId;
        this.isSubmitting = false;
        this.toastr.success('Product Created.');
      },
      error: () => {
        this.isSubmitting = false;
        this.toastr.error('Failed to create product');
      }
    });
  }
}
