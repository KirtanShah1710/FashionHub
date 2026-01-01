import { Component } from '@angular/core';
import { RegisterDto } from '../../shared/models/auth';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerForm!: FormGroup;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router
  ) {
    this.registerForm = this.fb.group(
      {
        name: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        age: [null, [Validators.required, Validators.min(1)]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required]
      },
      { validators: this.passwordMatchValidator }
    );
  }

  // ✅ INLINE VALIDATOR
  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;

    if (!password || !confirmPassword) return null;

    return password === confirmPassword
      ? null
      : { passwordMismatch: true };
  }

  onSubmit() {
    if (this.registerForm.invalid) return;

    this.loading = true;

    const data: RegisterDto = {
      name: this.registerForm.value.name,
      email: this.registerForm.value.email,
      age: this.registerForm.value.age,
      password: this.registerForm.value.password
    };

    this.authService.register(data).subscribe({
      next: () => {
        this.toastr.success('Registration successful. Please login.');
        this.router.navigate(['/auth/login']);
      },
      error: () => {
        this.loading = false;
        this.toastr.error('User already exists or invalid data');
      }
    });
  }
}
