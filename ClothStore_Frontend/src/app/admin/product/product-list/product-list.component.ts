import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ProductService } from '../../../shared/services/product.service';
import { CurrencyPipe, JsonPipe } from '@angular/common';
import { ProductVariantService } from '../../../shared/services/product-variant.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product-list',
  imports: [CurrencyPipe],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent {
  products: any[] = [];

  constructor(
    private productService: ProductService,
    private variantService: ProductVariantService,
    private router: Router,
    private toaster: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts() {
  this.productService.getAll().subscribe(products => {
    this.products = products;

    // 🔥 OPTION 2: load variants for each product
    this.products.forEach(p => {
      this.variantService.getByProductId(p.productId)
        .subscribe(variants => {
          p.variants = variants;
        });
    });
  });
}


  addProduct() {
    this.router.navigate(['/admin/products/create']);
  }

  editProduct(id: number) {
    this.router.navigate(['/admin/products/edit', id]);
  }

  viewDetail(id: number) {
    this.router.navigate(['/admin/products/detail', id]);
  }

  deleteProduct(id: number) {
   if (!confirm('Delete this product?')) return;

    this.productService.delete(id).subscribe(() => {
      this.loadProducts();
    });
    this.toaster.success("Product Deleted");
  }
}
