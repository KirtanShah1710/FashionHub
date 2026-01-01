import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
    loginForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private toastr: ToastrService,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.loginForm.invalid) return;

    this.authService.login(this.loginForm.value)
      .subscribe({
        next: (res) => {
          this.authService.saveToken(res.token);
          this.toastr.success('Login successful');

          const role = this.authService.getRole();
          role === 'Admin'
            ? this.router.navigate(['/admin'])
            : this.router.navigate(['/customer/products']);
        },
        error: (err) => {
          if (err.status === 401) {
            this.toastr.error('Invalid email or password');
          } else if (err.status === 0) {
            this.toastr.error('Server not reachable');
          } else {
            this.toastr.error('Something went wrong. Please try again');
          }
        }
      });
  }
}
