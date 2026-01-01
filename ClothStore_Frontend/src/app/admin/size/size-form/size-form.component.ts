import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Size } from '../../../shared/models/size.model';
import { SizeService } from '../../../shared/services/size.service';

@Component({
  selector: 'app-size-form',
  imports: [ReactiveFormsModule],
  templateUrl: './size-form.component.html',
  styleUrl: './size-form.component.css'
})
export class SizeFormComponent {
  form!: FormGroup;
  isEditMode = false;
  sizeId!: number;

  constructor(
    private fb: FormBuilder,
    private service: SizeService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.buildForm();

    this.sizeId = Number(this.route.snapshot.paramMap.get('id'));

    if (this.sizeId) {
      this.isEditMode = true;
      this.loadSize();
    }
  }

  private buildForm(): void {
    this.form = this.fb.group({
      sizeId: [0],
      name: ['', Validators.required]
    });
  }

  private loadSize(): void {
    this.service.getById(this.sizeId).subscribe({
      next: res => this.form.patchValue(res),
      error: () => this.toastr.error('Failed to load size')
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const data = this.form.value as Size;

    if (this.isEditMode) {
      this.service.update(this.sizeId, data).subscribe({
        next: () => {
          this.toastr.success('Size updated successfully');
          this.router.navigate(['/admin/sizes']);
        },
        error: err => this.toastr.error(err.error || 'Update failed')
      });
    } else {
      this.service.create(data).subscribe({
        next: () => {
          this.toastr.success('Size created successfully');
          this.router.navigate(['/admin/sizes']);
        },
        error: err => this.toastr.error(err.error || 'Create failed')
      });
    }
  }

}
