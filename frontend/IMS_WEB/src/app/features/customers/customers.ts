import { Component, Injector, runInInjectionContext, signal } from '@angular/core';
import { Customer } from '../../models/customers/customer';
import { Warehouse } from '../../models/warehouses/warehoue';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CustomerService } from './services/customer-service';
import { WarehouseService } from '../warehouses/services/warehouse-service';
import { Sidebar } from '../../shared/components/sidebar/sidebar';
import { CommonModule } from '@angular/common';
import { debounceTime, distinctUntilChanged, merge, switchMap } from 'rxjs';
import { toObservable } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-customers',
  imports: [Sidebar, ReactiveFormsModule, CommonModule, FormsModule],
  templateUrl: './customers.html',
  styleUrl: './customers.css',
})
export class Customers {
  customers = signal<Customer[]>([]);
  warehouses = signal<Warehouse[]>([]);
  customerForm: FormGroup;
  error = signal<string | null>(null);
  showModal = signal(false);
  errorMessage = '';
  successMessage = '';

  sidebarOpen = signal(false);

  searchTerm = signal<string>('');
  warehouseFilter = signal<string>('');
  constructor(
    private customerService: CustomerService,
    private fb: FormBuilder,
    private warehouseService: WarehouseService,
    private injector: Injector
  ) {
    this.customerForm = this.fb.group({
      id: [0],
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      address: ['', Validators.required],
      warehouseId: [0, Validators.required],
    });
  }

  ngOnInit(): void {
    this.loadCustomers();
    this.loadWarehouses();
  }
  private loadCustomers(): void {
    runInInjectionContext(this.injector, () => {
      const searchTerm$ = toObservable(this.searchTerm);
      const warehouseFilter$ = toObservable(this.warehouseFilter);
      merge(searchTerm$, warehouseFilter$)
        .pipe(
          debounceTime(300),
          distinctUntilChanged(),
          switchMap(() =>
            this.customerService.getCustomers(this.searchTerm(), this.warehouseFilter())
          )
        )
        .subscribe({
          next: (response) => {
            this.customers.set(response?.data ?? []);
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

  openModal(customer?: Customer) {
    if (customer) {
      this.customerForm.setValue({
        id: customer.id,
        name: customer.name,
        email: customer.email,
        phoneNumber: customer.phoneNumber,
        address: customer.address,
        warehouseId: customer.warehouseId,
      });
    } else {
      this.customerForm.reset({
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
  saveCustomer() {
    if (this.customerForm.invalid) {
      this.error.set('Please fill in all required fields.');
      return;
    }
    const customerData = this.customerForm.value;
    console.log(customerData);

    if (customerData.id) {
      this.customerService.updateCustomer(customerData).subscribe({
        next: () => {
          this.successMessage = 'Customer updated successfully.';
          this.loadCustomers();
          this.closeModal();
        },
      });
    } else {
      this.customerService.createCustomer(customerData).subscribe({
        next: () => {
          this.successMessage = 'Customer created successfully.';
          this.loadCustomers();
          this.closeModal();
        },
      });
    }
  }
  deleteCustomer(customerId: number) {
    this.customerService.deleteCustomer(customerId).subscribe({
      next: () => {
        this.successMessage = 'Customer deleted successfully.';
        this.loadCustomers();
      },
    });
  }
}
