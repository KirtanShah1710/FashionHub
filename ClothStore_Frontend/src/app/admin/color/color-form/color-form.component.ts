import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Color } from '../../../shared/models/color.model';
import { ColorService } from '../../../shared/services/color.service';

@Component({
  selector: 'app-color-form',
  imports: [ReactiveFormsModule],
  templateUrl: './color-form.component.html',
  styleUrl: './color-form.component.css'
})
export class ColorFormComponent {
  form!: FormGroup;
  isEditMode = false;
  colorId!: number;

  constructor(
    private fb: FormBuilder,
    private service: ColorService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.buildForm();

    this.colorId = Number(this.route.snapshot.paramMap.get('id'));

    if (this.colorId) {
      this.isEditMode = true;
      this.loadColor();
    }
  }

  private buildForm(): void {
    this.form = this.fb.group({
      colorId: [0],
      name: ['', Validators.required],
      hexCode: ['#000000'] // default black
    });
  }

  private loadColor(): void {
    this.service.getById(this.colorId).subscribe({
      next: res => this.form.patchValue(res),
      error: () => this.toastr.error('Failed to load color')
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const data = this.form.value as Color;

    if (this.isEditMode) {
      this.service.update(this.colorId, data).subscribe({
        next: () => {
          this.toastr.success('Color updated successfully');
          this.router.navigate(['/admin/colors']);
        },
        error: err => this.toastr.error(err.error || 'Update failed')
      });
    } else {
      this.service.create(data).subscribe({
        next: () => {
          this.toastr.success('Color created successfully');
          this.router.navigate(['/admin/colors']);
        },
        error: err => this.toastr.error(err.error || 'Create failed')
      });
    }
  }
}
