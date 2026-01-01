import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { ColorService } from '../../../shared/services/color.service';
import { SizeService } from '../../../shared/services/size.service';
import { ProductVariantService } from '../../../shared/services/product-variant.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product-variant-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './product-variant-form.component.html',
  styleUrls: ['./product-variant-form.component.css']
})
export class ProductVariantFormComponent {
  @Input() productId!: number;
  @Input() variantId?: number | null = null;
  @Output() saved = new EventEmitter<void>();

  variantForm!: FormGroup;
  isEditMode = false;

  sizes: any[] = [];
  colors: any[] = [];

  selectedVariantImage: File | null = null;
  imagePreview: string | null = null;

  submitting = false;

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder,
    private variantService: ProductVariantService,
    private sizeService: SizeService,
    private colorService: ColorService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadDropdowns();
  }

  private initForm(): void {
    this.variantForm = this.fb.group({
      sizeId: [null, Validators.required],
      colorId: [null, Validators.required],
      price: [null, Validators.required],
      stock: [null, Validators.required]
    });
  }

  private loadDropdowns(): void {
    this.sizeService.getAll().subscribe(sizes => this.sizes = sizes);
    this.colorService.getAll().subscribe(colors => {
      this.colors = colors;
      if (this.variantId) this.loadVariant();
    });
  }

  private loadVariant(): void {
    if (!this.variantId) return;
    this.isEditMode = true;

    this.variantService.getById(this.variantId).subscribe(res => {
      // Match size/color ID
      const matchedSize = this.sizes.find(s => s.name === res.size);
      const matchedColor = this.colors.find(c => c.name === res.color);

      this.variantForm.patchValue({
        sizeId: matchedSize?.sizeId ?? null,
        colorId: matchedColor?.colorId ?? null,
        price: res.price,
        stock: res.stock
      });

      // PREVIEW previous image
      if (res.imageUrl) {
        this.imagePreview = res.imageUrl.startsWith('http')
          ? res.imageUrl
          : `http://localhost:5276${res.imageUrl}`;
      }
    });
  }

  onImageChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length) {
      this.selectedVariantImage = input.files[0];

      const reader = new FileReader();
      reader.onload = () => this.imagePreview = reader.result as string;
      reader.readAsDataURL(this.selectedVariantImage);
    }
  }

  getColorHex(colorId: number | string): string {
    const color = this.colors.find(c => Number(c.colorId) === Number(colorId));
    return color?.hexCode ?? '#ffffff';
  }

  submitVariant(): void {
    if (this.variantForm.invalid || this.submitting) return;

    this.submitting = true;

    const formData = new FormData();
    formData.append('productId', this.productId.toString());
    formData.append('sizeId', this.variantForm.value.sizeId.toString());
    formData.append('colorId', this.variantForm.value.colorId.toString());
    formData.append('price', this.variantForm.value.price.toString());
    formData.append('stock', this.variantForm.value.stock.toString());

    if (this.selectedVariantImage) {
      formData.append('image', this.selectedVariantImage);
    }

    const request$ = this.isEditMode
      ? this.variantService.update(this.variantId!, formData)
      : this.variantService.create(formData);

    request$.subscribe({
      next: () => {
        this.submitting = false;
        this.saved.emit();
        if(this.isEditMode){
          this.toastr.success('Varient Edited!');
        }
        else{
          this.toastr.success('Varient Created!');
        }

      },
      error: err => {
        this.submitting = false;
        console.error(err);
        this.toastr.error('Variant save failed');
      }
    });
  }
}
