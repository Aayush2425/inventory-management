import { Component, Injector, runInInjectionContext, signal } from '@angular/core';
import { Supplier } from '../../models/suppilers/supplier';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Sidebar } from '../../shared/components/sidebar/sidebar';
import { SupplierService } from './services/supplier-service';
import { email } from '@angular/forms/signals';
import { Warehouse } from '../../models/warehouses/warehoue';
import { WarehouseService } from '../warehouses/services/warehouse-service';
import { toObservable } from '@angular/core/rxjs-interop';
import { debounceTime, distinctUntilChanged, merge, switchMap } from 'rxjs';

@Component({
  selector: 'app-suppliers',
  imports: [ReactiveFormsModule, CommonModule, Sidebar, FormsModule],
  templateUrl: './suppliers.html',
  styleUrl: './suppliers.css',
})
export class Suppliers {
  suppliers = signal<Supplier[]>([]);
  warehouses = signal<Warehouse[]>([]);
  supplierForm: FormGroup;
  error = signal<string | null>(null);
  showModal = signal(false);
  errorMessage = '';
  successMessage = '';

  searchTerm = signal<string>('');
  warehouseFilter = signal<string>('');

  constructor(
    private supplierService: SupplierService,
    private fb: FormBuilder,
    private warehouseService: WarehouseService,
    private injector: Injector
  ) {
    this.supplierForm = this.fb.group({
      id: [0],
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      address: ['', Validators.required],
      warehouseId: [0, Validators.required],
    });
  }

  ngOnInit(): void {
    this.loadSuppliers();
    this.loadWarehouses();
  }
  private loadSuppliers(): void {
    runInInjectionContext(this.injector, () => {
      const searchTerm$ = toObservable(this.searchTerm);
      const warehouseFilter$ = toObservable(this.warehouseFilter);
      merge(searchTerm$, warehouseFilter$)
        .pipe(
          debounceTime(300),
          distinctUntilChanged(),
          switchMap(() =>
            this.supplierService.getSuppliers(this.searchTerm(), this.warehouseFilter())
          )
        )
        .subscribe({
          next: (response) => {
            this.suppliers.set(response?.data ?? []);
          },
        });
    });
  }
  loadWarehouses() {
    this.warehouseService.getWarehouses().subscribe({
      next: (response) => {
        this.warehouses.set(response?.data ?? []);
      },
      error: (error) => {
        this.error.set('Failed to load warehouses.');
      },
    });
  }

  openModal(supplier?: Supplier) {
    if (supplier) {
      this.supplierForm.setValue({
        id: supplier.id,
        name: supplier.name,
        email: supplier.email,
        phoneNumber: supplier.phoneNumber,
        address: supplier.address,
        warehouseId: supplier.warehouseId,
      });
    } else {
      this.supplierForm.reset({
        id: 0,
        name: '',
        email: '',
        phoneNumber: '',
        address: '',
        warehouseId: 0,
      });
    }
    this.showModal.set(true);
  }
  closeModal() {
    this.showModal.set(false);
    this.error.set(null);
  }
  saveSupplier() {
    if (this.supplierForm.invalid) {
      this.error.set('Please fill in all required fields.');
      return;
    }
    const supplierData = this.supplierForm.value;
    if (supplierData.id) {
      this.supplierService.updateSupplier(supplierData).subscribe({
        next: () => {
          this.successMessage = 'Supplier updated successfully.';
          this.loadSuppliers();
          this.closeModal();
        },
      });
    } else {
      this.supplierService.createSupplier(supplierData).subscribe({
        next: () => {
          this.successMessage = 'Supplier created successfully.';
          this.loadSuppliers();
          this.closeModal();
        },
      });
    }
  }
  deleteSupplier(supplierId: number) {
    this.supplierService.deleteSupplier(supplierId).subscribe({
      next: () => {
        this.successMessage = 'Supplier deleted successfully.';
        this.loadSuppliers();
      },
    });
  }
}
