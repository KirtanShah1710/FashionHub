import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

import { ProductService } from '../../shared/services/product.service';
import { ProductVariantService } from '../../shared/services/product-variant.service';
import { OrderService } from '../../shared/services/order.service';
import { ToastrService } from 'ngx-toastr';
import { ProductListComponent } from '../product-list/product-list.component';

//payment
declare var Razorpay: any;

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.css'
})

export class ProductDetailComponent implements OnInit {

  productId!: number;
  product: any = null;
  variants: any[] = [];

  selectedColor!: string;
  selectedSize!: string;
  selectedVariant: any;

  colors: string[] = [];
  sizes: string[] = [];

  imageBaseUrl = 'http://localhost:5276';
  currentTransformOrigin: string = 'center center'; 

  // order
  quantity: number = 1;

  constructor(
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private productService: ProductService,
    private variantService: ProductVariantService,
    private orderService: OrderService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.productId = Number(this.route.snapshot.paramMap.get('id'));

    this.route.queryParams.subscribe(params => {
    this.selectedColor = params['color'];
    this.selectedSize = params['size'];
  });

    this.loadProduct();
  }

  // loadProduct() {
  //   this.productService.getById(this.productId).subscribe(prod => {
  //     this.product = prod;

  //     this.variantService.getByProductId(this.productId).subscribe(vs => {
  //       this.variants = vs;


  //       if (this.variants.length) {
  //         this.colors = [...new Set(this.variants.map(v => v.color))];
  //         this.sizes = [...new Set(this.variants.map(v => v.size))];

  //         this.selectedVariant = this.variants[0];
  //         this.selectedColor = this.selectedVariant.color;
  //         this.selectedSize = this.selectedVariant.size;
  //       }
  //     });

  //   });
  // }

  loadProduct() {
  this.productService.getById(this.productId).subscribe(prod => {
    this.product = prod;

    this.variantService.getByProductId(this.productId).subscribe(vs => {
      this.variants = vs;

      if (!this.variants.length) return;

      this.colors = [...new Set(this.variants.map(v => v.color))];
      this.sizes = [...new Set(this.variants.map(v => v.size))];

      // ✅ IF coming from product list
      if (this.selectedColor && this.selectedSize) {
        const matched = this.variants.find(
          v =>
            v.color === this.selectedColor &&
            v.size === this.selectedSize
        );

        if (matched) {
          this.selectedVariant = matched;
        } else {
          this.selectedVariant = this.variants[0];
          this.selectedColor = this.selectedVariant.color;
          this.selectedSize = this.selectedVariant.size;
        }
      }
      // ✅ DEFAULT LOAD
      else {
        this.selectedVariant = this.variants[0];
        this.selectedColor = this.selectedVariant.color;
        this.selectedSize = this.selectedVariant.size;
      }
    });
  });
}


  // ================= COLOR CLICK =================
  selectColor(color: string) {
    this.selectedColor = color;

    const availableSizes = this.variants
      .filter(v => v.color === color)
      .map(v => v.size);

    if (!availableSizes.includes(this.selectedSize)) {
      this.selectedSize = availableSizes[0];
    }

    this.updateVariant();
  }

  // ================= SIZE CLICK =================
  selectSize(size: string) {
    this.selectedSize = size;

    const availableColors = this.variants
      .filter(v => v.size === size)
      .map(v => v.color);

    if (!availableColors.includes(this.selectedColor)) {
      this.selectedColor = availableColors[0];
    }

    this.updateVariant();
  }

  // ================= UPDATE VARIANT =================
  updateVariant() {
    const variant = this.variants.find(
      v => v.color === this.selectedColor && v.size === this.selectedSize
    );
    if (variant) {
      this.selectedVariant = variant;
    }
  }

  // ================= AVAILABILITY =================
  isColorAvailable(color: string): boolean {
    return this.variants.some(
      v => v.color === color && v.size === this.selectedSize
    );
  }

  isSizeAvailable(size: string): boolean {
    return this.variants.some(
      v => v.size === size && v.color === this.selectedColor
    );
  }

  // ================= IMAGE BY COLOR =================
  getImageByColor(color: string): string {
    const v = this.variants.find(
      x => x.color === color && x.size === this.selectedSize
    ) || this.variants.find(x => x.color === color);

    return this.imageBaseUrl + (v?.imageUrl || this.product.productImageUrl);
  }


  // order logic
  // Increment quantity
  increaseQty() {
    if (!this.selectedVariant) return;

    if (this.quantity < this.selectedVariant.stock) {
      this.quantity++;
    }
  }

  // Decrement quantity
  decreaseQty() {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  // Total amount calculation
  get totalAmount(): number {
    if (!this.selectedVariant) return 0;
    return this.quantity * this.selectedVariant.price;
  }

  // Place order
    placeOrder() {
    if (!this.selectedVariant) {
      alert('Please select color and size');
      return;
    }

    const payload = {
      productVariantId: this.selectedVariant.productVariantId ?? this.selectedVariant.id,
      quantity: this.quantity
    };

    // 1️⃣ Create payment order in backend
    this.orderService.startPayment(payload).subscribe({
      next: (order) => {
        this.openRazorpay(order);
      },
      error: () => {
        alert('Failed to initiate payment');
      }
    });
  }
  openRazorpay(order: any) {
  const options = {
    key: 'rzp_test_RudjBsAPPgxGqd',
    amount: order.totalAmount,
    currency: 'INR',
    name: 'Cloth Store',
    description: 'Order Payment',

    order_id: order.razorpayOrderId, // ✅ REQUIRED

    handler: (response: any) => {
      console.log('FRONTEND ORDER ID (DB):', order.razorpayOrderId);
      console.log('FRONTEND ORDER ID (RP):', response.razorpay_order_id);
      console.log('PAYMENT ID:', response.razorpay_payment_id);
      console.log('SIGNATURE:', response.razorpay_signature);
      this.verifyPayment(order.orderId, response);
    },

    prefill: {
      name: 'Customer',
      email: 'customer@test.com',
      contact: '9999999999'
    },

    theme: { color: '#3399cc' }
  };

  const rzp = new Razorpay(options);
  rzp.open();
}

  verifyPayment(orderId: number, response: any) {
    const payload = {
      orderId: orderId,
      razorpayPaymentId: response.razorpay_payment_id,
      razorpayOrderId: response.razorpay_order_id,
      razorpaySignature: response.razorpay_signature
    };

    this.orderService.verifyPayment(payload).subscribe({
      next: () => {
        this.toastr.success('Payment successful!',
        'Order placed successfully');
        this.quantity = 1;
        this.router.navigate(['/my-orders']);
      },
      error: () => {
        this.toastr.error('Payment verification failed');
      }
    });
  }
  // Handle mouse movement to track position
  onMouseMove(event: MouseEvent): void {
    const { offsetX, offsetY } = event;
    const imageWrapper = event.currentTarget as HTMLElement;

    const xPercent = (offsetX / imageWrapper.offsetWidth) * 100;
    const yPercent = (offsetY / imageWrapper.offsetHeight) * 100;

    this.currentTransformOrigin = `${xPercent}% ${yPercent}%`;
  }

  // Reset zoom when mouse leaves the image
  onMouseLeave(): void {
    this.currentTransformOrigin = 'center center';
  }
}
