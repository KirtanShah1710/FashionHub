import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { forkJoin } from 'rxjs';

import { ProductService } from '../../../shared/services/product.service';
import { ProductVariantService } from '../../../shared/services/product-variant.service';
import { ProductVariantFormComponent } from '../product-variant-form/product-variant-form.component';
import { GenderService } from '../../../shared/services/gender.service';
import { ProductCategorService } from '../../../shared/services/product-categor.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product-edit',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, ProductVariantFormComponent, CurrencyPipe],
  templateUrl: './product-edit.component.html',
  styleUrl: './product-edit.component.css'
})
export class ProductEditComponent implements OnInit {

  productId!: number;
  productForm!: FormGroup;

  productCategories: any[] = [];
  genderCategories: any[] = [];

  // ---------- IMAGE ----------
  selectedImage: File | null = null;
  productImagePreview: string | null = null;

  // ---------- VARIANTS ----------
  variants: any[] = [];
  showVariantForm = false;
  editVariantId?: number;

  constructor(
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private productService: ProductService,
    private variantService: ProductVariantService,
    private productCategoryService: ProductCategorService,
    private genderCategoryService: GenderService
  ) {}

  ngOnInit(): void {
    this.productId = Number(this.route.snapshot.paramMap.get('id'));
    this.buildForm();

    // Load dropdowns first, then product
    forkJoin({
      categories: this.productCategoryService.getAll(),
      genders: this.genderCategoryService.getAll()
    }).subscribe(({ categories, genders }) => {
      this.productCategories = categories;
      this.genderCategories = genders;

      // Now safe to load product
      this.loadProduct();
    });

    this.loadVariants();
  }

  // ---------- FORM ----------
  buildForm() {
    this.productForm = this.fb.group({
      productName: ['', Validators.required],
      description: [''],
      productCategoryId: [null, Validators.required],
      genderCategoryId: [null, Validators.required]
    });
  }

  // ---------- LOAD PRODUCT ----------
  loadProduct() {
  this.productService.getById(this.productId).subscribe(res => {

    // find matching category by name
    const category = this.productCategories.find(
      c => c.name === res.productCategory
    );

    // find matching gender by name
    const gender = this.genderCategories.find(
      g => g.name === res.genderCategory
    );

    this.productForm.patchValue({
      productName: res.productName,
      description: res.description,
      productCategoryId: category ? category.productCategoryId : null,
      genderCategoryId: gender ? gender.genderCategoryId : null
    });

    // image preview
    if (res.productImageUrl) {
      this.productImagePreview = `http://localhost:5276${res.productImageUrl}`;
    }
  });
}


  // ---------- IMAGE CHANGE ----------
  onImageChange(event: any) {
    const file = event.target.files?.[0];
    if (!file) return;

    this.selectedImage = file;

    const reader = new FileReader();
    reader.onload = () => {
      this.productImagePreview = reader.result as string;
    };
    reader.readAsDataURL(file);
  }

  // ---------- UPDATE PRODUCT ----------
  updateProduct() {
    if (this.productForm.invalid) return;

    const formData = new FormData();

    formData.append('productName', this.productForm.value.productName);
    formData.append('description', this.productForm.value.description);
    formData.append('productCategoryId', this.productForm.value.productCategoryId);
    formData.append('genderCategoryId', this.productForm.value.genderCategoryId);

    if (this.selectedImage) {
      formData.append('image', this.selectedImage);
    }

    this.productService.update(this.productId, formData)
      .subscribe(() => {
        this.toastr.success("Product updated successfully");
      });
  }

  // ================= VARIANTS =================

  loadVariants() {
    this.variantService.getByProductId(this.productId)
      .subscribe(res => this.variants = res);
  }

  addVariant() {
    this.editVariantId = undefined;
    this.showVariantForm = true;
  }

  editVariant(id: number) {
    this.editVariantId = id;
    this.showVariantForm = true;
  }

  deleteVariant(id: number) {
    if (!confirm('Delete variant?')) return;

    this.variantService.delete(id)
      .subscribe(() => this.loadVariants());
      this.toastr.success('Varient Deleted!');
  }

  onVariantSaved() {
    this.showVariantForm = false;
    this.loadVariants();
  }
}

