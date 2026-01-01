import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Color } from '../../../shared/models/color.model';
import { ColorService } from '../../../shared/services/color.service';

@Component({
  selector: 'app-color-list',
  imports: [],
  templateUrl: './color-list.component.html',
  styleUrl: './color-list.component.css'
})
export class ColorListComponent {
  colors: Color[] = [];
  loading = true;

  constructor(
    private service: ColorService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadColors();
  }

  loadColors(): void {
    this.loading = true;
    this.service.getAll().subscribe({
      next: res => {
        this.colors = res;
        this.loading = false;
      },
      error: () => {
        this.toastr.error('Failed to load colors');
        this.loading = false;
      }
    });
  }

  edit(id: number): void {
    this.router.navigate([`/admin/colors/edit/${id}`]);
  }

  delete(id: number): void {
    if (confirm('Are you sure you want to delete this color?')) {
      this.service.delete(id).subscribe({
        next: () => {
          this.toastr.success('Color deleted successfully');
          this.loadColors();
        },
        error: () => this.toastr.error('Delete failed')
      });
    }
  }

  create(): void {
    this.router.navigate(['/admin/colors/create']);
  }
}
