import { Component, effect, Injector, OnInit, runInInjectionContext, signal } from '@angular/core';
import { ProductServices } from './services/product-services';
import {
  Product,
  productWithInventory,
  ProductWithInventoryCreate,
} from '../../models/products/product';
import { decodeJwt } from '../../core/utils/jwt.util';
import { Sidebar } from '../../shared/components/sidebar/sidebar';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ApiResponse } from '../../models/apiResponse';
import { CategoryService } from '../categories/services/category-service';
import { Category } from '../../models/categories/category';
import { Warehouse } from '../../models/warehouses/warehoue';
import { WarehouseService } from '../warehouses/services/warehouse-service';
import { toObservable } from '@angular/core/rxjs-interop';
import { debounceTime, distinctUntilChanged, merge, switchMap } from 'rxjs';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [Sidebar, CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './products.html',
  styleUrl: './products.css',
})
export class Products implements OnInit {
  products = signal<Product[]>([]);
  categories = signal<Category[]>([]);
  warehouses = signal<Warehouse[]>([]);
  productWithInventoryForm: FormGroup;
  error = signal<string | null>(null);
  showModal = signal(false);
  errorMessage = '';
  successMessage = '';
  onupdateModal = signal(false);

  searchTerm = signal<string>('');
  categoryFilter = signal<string>('');

  constructor(
    private productService: ProductServices,
    private fb: FormBuilder,
    private categoryService: CategoryService,
    private warehouseService: WarehouseService,
    private router: Router,
    private injector: Injector
  ) {
    this.productWithInventoryForm = this.fb.group({
      product: this.fb.group({
        id: [0],
        name: ['', Validators.required],
        sku: ['', Validators.required],
        categoryId: [0, Validators.required],
        price: [0, [Validators.required, Validators.min(0)]],
        description: [''],
        isActive: [true],
        userId: [0, Validators.required],
      }),

      inventory: this.fb.group({
        warehouseId: [0, Validators.required],
        quantity: [0, [Validators.required, Validators.min(0)]],
        reorderLevel: [0, [Validators.required, Validators.min(0)]],
        reorderQuantity: [0, [Validators.required, Validators.min(0)]],
      }),
    });
  }

  ngOnInit(): void {
    this.loadProducts();
  }

  private loadProducts(): void {
    runInInjectionContext(this.injector, () => {
      const searchTerm$ = toObservable(this.searchTerm);
      const categoryFilter$ = toObservable(this.categoryFilter);

      merge(searchTerm$, categoryFilter$)
        .pipe(
          debounceTime(400),
          distinctUntilChanged(),
          switchMap(() => this.productService.getProducts(this.searchTerm(), this.categoryFilter()))
        )
        .subscribe({
          next: (response) => this.products.set(response?.data ?? []),
          error: (err) => {
            console.error('API ERROR:', err);
            this.error.set('Failed to load products');
          },
        });
    });

    this.fetchCategories();
    this.fetchWarehouses();
  }
  fetchProducts(): void {
    this.productService.getProducts(this.searchTerm(), this.categoryFilter()).subscribe({
      next: (response) => {
        console.log(response);

        this.products.set(response?.data ?? []);
        console.log(this.products());
      },
      error: (err) => {
        console.error('API ERROR:', err);
        this.error.set('Failed to load products');
      },
    });
  }
  private fetchCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (response) => {
        this.categories.set(response?.data ?? []);
      },
      error: (err) => {
        console.error('API ERROR:', err);
        this.error.set('Failed to load categories');
      },
    });
  }
  private fetchWarehouses(): void {
    this.warehouseService.getWarehouses().subscribe({
      next: (response) => {
        this.warehouses.set(response?.data ?? []);
      },
      error: (err) => {
        console.error('API ERROR:', err);
        this.error.set('Failed to load warehouses');
      },
    });
  }

  onCreateProduct(): void {
    if (this.productWithInventoryForm.invalid) {
      this.productWithInventoryForm.markAllAsTouched();

      return;
    }
    this.resetMessages();
    console.log(this.productWithInventoryForm.value);

    this.productService.createProduct(this.productWithInventoryForm.value).subscribe({
      next: (res: ApiResponse<ProductWithInventoryCreate>) => {
        this.loadProducts();
        this.handleSuccess(res, 'Product Created successful');
        this.toggleModal();
      },
      error: (err: any) => this.handleError(err),
    });
  }
  onUpdateProductModal(product: any): void {
    console.log(product.inventoryItems);

    this.productWithInventoryForm.patchValue({
      product: {
        id: product.id,
        name: product.name,
        sku: product.sku,
        categoryId: product.categoryId,
        price: product.price,
        description: product.description,
        isActive: product.isActive,
        // userId: product.userId,
      },
      inventory: product.inventoryItems
        ? {
            warehouseId: product.inventoryItems[0].warehouseId,
            quantity: product.inventoryItems[0].quantity,
            reorderLevel: product.inventoryItems[0].reorderLevel,
            reorderQuantity: product.inventoryItems[0].reorderQuantity,
          }
        : {
            warehouseId: 0,
            quantity: 0,
            reorderLevel: 0,
            reorderQuantity: 0,
          },
    });
    this.onupdateModal.set(true);
    this.toggleModal();
  }
  onUpdateProduct(): void {
    if (this.productWithInventoryForm.invalid) {
      this.productWithInventoryForm.markAllAsTouched();
      return;
    }
    console.log(this.productWithInventoryForm.value);

    this.resetMessages();
    this.productService.updateProduct(this.productWithInventoryForm.value).subscribe({
      next: (res: ApiResponse<ProductWithInventoryCreate>) => {
        this.loadProducts();
        this.handleSuccess(res, 'Product Updated successful');
        this.showModal.set(false);
      },
      error: (err: any) => this.handleError(err),
    });
  }
  onDeleteProduct(productId: number): void {
    console.log('Delete product with ID:', productId);
    this.productService.onDeleteProduct(productId).subscribe({
      next: (res: ApiResponse<null>) => {
        this.loadProducts();
      },
      error: (err: any) => this.handleError(err),
    });
  }

  private handleSuccess(res: ApiResponse<ProductWithInventoryCreate>, message: string) {
    this.successMessage = message;
  }

  private handleError(err: any) {
    this.errorMessage = err?.error?.message || 'Login failed';
  }

  private resetMessages() {
    this.errorMessage = '';
    this.successMessage = '';
  }
  toggleModal() {
    if (this.showModal() == false) {
      this.showModal.set(true);
    } else {
      this.showModal.set(false);
      this.onupdateModal.set(false);
      this.productWithInventoryForm.reset();
    }
  }
}
