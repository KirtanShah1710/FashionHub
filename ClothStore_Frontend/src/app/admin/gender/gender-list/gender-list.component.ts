import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { GenderCategory } from '../../../shared/models/gender.model';
import { GenderService } from '../../../shared/services/gender.service';

@Component({
  selector: 'app-gender-list',
  imports: [],
  templateUrl: './gender-list.component.html',
  styleUrl: './gender-list.component.css'
})
export class GenderListComponent {
  genders: GenderCategory[] = [];
  loading = true;

  constructor(
    private service: GenderService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadGenders();
  }

  loadGenders(): void {
    this.loading = true;
    this.service.getAll().subscribe({
      next: res => {
        this.genders = res;
        this.loading = false;
      },
      error: () => {
        this.toastr.error('Failed to load genders');
        this.loading = false;
      }
    });
  }

  edit(id: number): void {
    this.router.navigate([`/admin/genders/edit/${id}`]);
  }

  delete(id: number): void {
    if (confirm('Are you sure you want to delete this gender?')) {
      this.service.delete(id).subscribe({
        next: () => {
          this.toastr.success('Gender deleted successfully');
          this.loadGenders();
        },
        error: () => this.toastr.error('Delete failed')
      });
    }
  }

  create(): void {
    this.router.navigate(['/admin/genders/create']);
  }
}
