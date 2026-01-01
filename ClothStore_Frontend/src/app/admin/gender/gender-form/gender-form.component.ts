import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { GenderCategory } from '../../../shared/models/gender.model';
import { GenderService } from '../../../shared/services/gender.service';

@Component({
  selector: 'app-gender-form',
  imports: [ReactiveFormsModule],
  templateUrl: './gender-form.component.html',
  styleUrl: './gender-form.component.css'
})
export class GenderFormComponent {
  form!: FormGroup;
  isEditMode = false;
  genderId!: number;

  constructor(
    private fb: FormBuilder,
    private service: GenderService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.buildForm();

    this.genderId = Number(this.route.snapshot.paramMap.get('id'));

    if (this.genderId) {
      this.isEditMode = true;
      this.loadGender();
    }
  }

  private buildForm(): void {
    this.form = this.fb.group({
      genderCategoryId: [0],
      name: ['', Validators.required]
    });
  }

  private loadGender(): void {
    this.service.getById(this.genderId).subscribe({
      next: res => this.form.patchValue(res),
      error: () => this.toastr.error('Failed to load gender')
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const data = this.form.value as GenderCategory;

    if (this.isEditMode) {
      this.service.update(this.genderId, data).subscribe({
        next: () => {
          this.toastr.success('Gender updated successfully');
          this.router.navigate(['/admin/genders']);
        },
        error: err => this.toastr.error(err.error || 'Update failed')
      });
    } else {
      this.service.create(data).subscribe({
        next: () => {
          this.toastr.success('Gender created successfully');
          this.router.navigate(['/admin/genders']);
        },
        error: err => this.toastr.error(err.error || 'Create failed')
      });
    }
  }
}
