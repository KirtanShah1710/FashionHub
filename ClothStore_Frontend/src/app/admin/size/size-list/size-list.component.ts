import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Size } from '../../../shared/models/size.model';
import { SizeService } from '../../../shared/services/size.service';

@Component({
  selector: 'app-size-list',
  imports: [],
  templateUrl: './size-list.component.html',
  styleUrl: './size-list.component.css'
})
export class SizeListComponent {
  sizes: Size[] = [];
  loading = true;

  constructor(
    private service: SizeService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadSizes();
  }

  loadSizes(): void {
    this.loading = true;
    this.service.getAll().subscribe({
      next: res => {
        this.sizes = res;
        this.loading = false;
      },
      error: () => {
        this.toastr.error('Failed to load sizes');
        this.loading = false;
      }
    });
  }

  edit(id: number): void {
    this.router.navigate([`/admin/sizes/edit/${id}`]);
  }

  delete(id: number): void {
    if (confirm('Are you sure you want to delete this size?')) {
      this.service.delete(id).subscribe({
        next: () => {
          this.toastr.success('Size deleted successfully');
          this.loadSizes();
        },
        error: () => this.toastr.error('Delete failed')
      });
    }
  }

  create(): void {
    this.router.navigate(['/admin/sizes/create']);
  }
}
