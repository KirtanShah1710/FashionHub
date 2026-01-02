import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ProductCatalogService } from '../../shared/services/product-catalog.service';
import { GenderService } from '../../shared/services/gender.service';
import { ColorService } from '../../shared/services/color.service';
import { SizeService } from '../../shared/services/size.service';
import { ProductCategorService } from '../../shared/services/product-categor.service';
import { ProductVariantService } from '../../shared/services/product-variant.service';
import { CurrencyPipe, JsonPipe } from '@angular/common';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [FormsModule, RouterLink, CurrencyPipe, JsonPipe],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {

  imageBaseUrl = 'http://localhost:5276';

  products: any[] = [];
  meta: any;
  maxPrice: number = 0;

  minPercent = 0;
  maxPercent = 100;

   // Define the collapsed state for each filter section
  isCollapsed: { [key: string]: boolean } = {
    category: true,
    gender: true,
    size: true,
    color: true
  };

  // Toggle function to show/hide filter sections
  toggleFilter(filter: string) {
    this.isCollapsed[filter] = !this.isCollapsed[filter];
  }

  // ===== MASTER DATA =====
  categories: any[] = [];
  genders: any[] = [];
  colors: any[] = [];
  sizes: any[] = [];

  // ===== FILTER STATE =====
  filters = {
    search: '',
    productCategoryId: [] as number[],
    genderCategoryId: [] as number[],
    colorId: [] as number[],
    sizeId: [] as number[],
    minPrice: '' as any,
    maxPrice: '' as any,
    pageNumber: 1,
    pageSize: 12
  };

  constructor(
    private catalogService: ProductCatalogService,
    private categoryService: ProductCategorService,
    private genderService: GenderService,
    private colorService: ColorService,
    private sizeService: SizeService,
    private variantService: ProductVariantService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadSidebarData();
    this.loadCatalog();
    this.variantService.getAll().subscribe(
      (variants) => {
        this.maxPrice = Math.max(...variants.map(v => v.price));  
    });
  }

  // ===== LOAD MASTER DATA =====
  loadSidebarData(): void {
    this.categoryService.getAll().subscribe(r => this.categories = r);
    this.genderService.getAll().subscribe(r => this.genders = r);
    this.colorService.getAll().subscribe(r => this.colors = r);
    this.sizeService.getAll().subscribe(r => this.sizes = r);
  }

  // ===== LOAD CATALOG =====
  loadCatalog(): void {
    console.log(this.products);
    
    this.catalogService.getCatalog(this.filters).subscribe(res => {
      this.products = res.data;
      this.meta = res.meta;
      console.log('Filters:', JSON.stringify(this.filters));
    });
  }

  // ===== FILTER TOGGLES =====
  toggleArray(arr: number[], id: number, checked: boolean) {
    checked ? arr.push(id) : arr.splice(arr.indexOf(id), 1);
    this.filters.pageNumber = 1;
    this.loadCatalog();
  }

  onSearchChange() {
    this.filters.pageNumber = 1;
    this.loadCatalog();
  }

  onPriceChange() {
    if (this.filters.minPrice > this.filters.maxPrice) {
      const temp = this.filters.minPrice;
      this.filters.minPrice = this.filters.maxPrice;
      this.filters.maxPrice = temp;
    }

    if(this.filters.maxPrice && !this.filters.minPrice)
    {
      this.filters.minPrice = 0;
    }

    this.minPercent = (this.filters.minPrice / this.maxPrice) * 100;
    this.maxPercent = (this.filters.maxPrice / this.maxPrice) * 100;

    this.filters.pageNumber = 1;
    this.loadCatalog();
  }


  goToDetail(productId: number, color?: string, size?: string) {
    this.router.navigate(
      ['/customer/products', productId],
      { queryParams: { color, size } }
    );
  }

  resetPriceRange() {
    this.filters.minPrice = '';
    this.filters.maxPrice = '';
    this.filters.pageNumber = 1;
    this.loadCatalog();
  }
}
