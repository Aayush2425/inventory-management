import { Component, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormArray,
  Validators,
  ReactiveFormsModule,
  FormsModule,
} from '@angular/forms';
import { Sidebar } from '../../shared/components/sidebar/sidebar';
import { SalesorderService } from './services/salesorder-service';
import { CustomerService } from '../customers/services/customer-service';
import { ProductServices } from '../products/services/product-services';

import { Customer } from '../../models/customers/customer';
import { Product } from '../../models/products/product';
import { SalesOrder, SalesOrderWithItems } from '../../models/salesorders/salesorder';
@Component({
  selector: 'app-salesorders',
  imports: [Sidebar, ReactiveFormsModule, FormsModule],
  templateUrl: './salesorders.html',
  styleUrl: './salesorders.css',
})
export class Salesorders {
  salesOrders = signal<SalesOrder[]>([]);
  customers = signal<Customer[]>([]);
  products = signal<Product[]>([]);
  salesOrderForm!: FormGroup;
  error = signal<string | null>(null);
  showModal = signal(false);
  errorMessage = '';
  successMessage = '';

  searchTerm = signal<string>('');
  customerFilter = signal<string>('');

  constructor(
    private fb: FormBuilder,
    private customerService: CustomerService,
    private productService: ProductServices,
    private salesOrderService: SalesorderService
  ) {
    this.salesOrderForm = this.fb.group({
      salesOrder: this.fb.group({
        id: [0],
        orderNumber: ['', Validators.required],
        customerId: [0, Validators.required],
        orderDate: [new Date(), Validators.required],
        isShipped: [false, Validators.required],
        discount: [0, [Validators.required, Validators.min(0)]],
        tax: [0, [Validators.required, Validators.min(0)]],
        totalAmount: [0, [Validators.required, Validators.min(0)]],
      }),

      items: this.fb.array([]),
    });
  }

  ngOnInit(): void {
    this.loadCustomers();
    this.loadProducts();
  }

  loadCustomers() {
    this.customerService.getCustomers('', '').subscribe({
      next: (response) => {
        this.customers.set(response?.data ?? []);
        this.loadSalesOrders();
      },
      error: (error) => {
        this.error.set('Failed to load customers.');
      },
    });
  }

  loadProducts() {
    this.productService.getProducts().subscribe({
      next: (response) => {
        this.products.set(response?.data ?? []);
      },
      error: (error) => {
        this.error.set('Failed to load products');
      },
    });
  }

  loadSalesOrders() {
    this.salesOrderService.getAllSalesOrders().subscribe({
      next: (response) => {
        this.salesOrders.set(response.data ?? []);
      },
      error: (error) => {
        this.error.set('Failed to load sales orders');
      },
    });
  }

  get items(): FormArray {
    return this.salesOrderForm.get('items') as FormArray;
  }

  createItemForm(): FormGroup {
    return this.fb.group({
      productId: [0, Validators.required],
      quantity: [0, [Validators.required, Validators.min(1)]],
      unitPrice: [0, [Validators.required, Validators.min(0)]],
      warehouseId: [0, Validators.required],
      status: ['Pending'],
    });
  }

  addItem(): void {
    this.items.push(this.createItemForm());
  }

  removeItem(index: number): void {
    this.items.removeAt(index);
  }

  onSubmit(): void {
    if (this.salesOrderForm.valid) {
      const salesOrderData = this.salesOrderForm.value;
      const salesOrderId = salesOrderData.salesOrder.id;

      if (salesOrderId && salesOrderId !== 0) {
        this.updateSalesOrder(salesOrderData);
      } else {
        this.createSalesOrder(salesOrderData);
      }
    }
  }

  createSalesOrder(salesOrderData: any): void {
    const payload: SalesOrderWithItems = {
      salesOrder: {
        id: Number(salesOrderData.salesOrder.id ?? 0),
        orderNumber: salesOrderData.salesOrder.orderNumber,
        customerId: Number(salesOrderData.salesOrder.customerId),
        orderDate: salesOrderData.salesOrder.orderDate,
        isShipped: salesOrderData.salesOrder.isShipped === true,
        discount: Number(salesOrderData.salesOrder.discount),
        tax: Number(salesOrderData.salesOrder.tax),
        totalAmount: Number(salesOrderData.salesOrder.totalAmount),
      },
      items: salesOrderData.items.map((item: any) => ({
        productId: Number(item.productId),
        quantity: Number(item.quantity),
        unitPrice: Number(item.unitPrice),
        warehouseId: Number(item.warehouseId),
        status: item.status,
      })),
    };

    this.salesOrderService.createSalesOrder(payload).subscribe({
      next: (response) => {
        this.successMessage = 'Sales order created successfully!';
        this.error.set(null);
        this.closeModal();
        this.loadSalesOrders();
      },
      error: (error) => {
        this.errorMessage = 'Failed to create sales order.';
        this.error.set(error);
      },
    });
  }

  updateSalesOrder(salesOrderData: any): void {
    const salesOrderId = salesOrderData.salesOrder.id;

    if (!salesOrderId) {
      this.error.set('Sales order ID is required for update.');
      return;
    }

    const payload: SalesOrderWithItems = {
      salesOrder: {
        id: Number(salesOrderId),
        orderNumber: salesOrderData.salesOrder.orderNumber,
        customerId: Number(salesOrderData.salesOrder.customerId),
        orderDate: salesOrderData.salesOrder.orderDate,
        isShipped: salesOrderData.salesOrder.isShipped === true,
        discount: Number(salesOrderData.salesOrder.discount),
        tax: Number(salesOrderData.salesOrder.tax),
        totalAmount: Number(salesOrderData.salesOrder.totalAmount),
      },
      items: salesOrderData.items.map((item: any) => ({
        productId: Number(item.productId),
        quantity: Number(item.quantity),
        unitPrice: Number(item.unitPrice),
        warehouseId: Number(item.warehouseId),
        status: item.status,
      })),
    };

    this.salesOrderService.updateSalesOrder(salesOrderId, payload).subscribe({
      next: (response) => {
        this.successMessage = 'Sales order updated successfully!';
        this.error.set(null);
        this.closeModal();
        this.loadSalesOrders();
      },
      error: (error) => {
        this.errorMessage = 'Failed to update sales order.';
        this.error.set(error);
      },
    });
  }

  deleteSalesOrder(id: number): void {
    if (confirm('Are you sure you want to delete this sales order?')) {
      this.salesOrderService.deleteSalesOrder(id).subscribe({
        next: (response) => {
          this.successMessage = 'Sales order deleted successfully!';
          this.error.set(null);
          this.loadSalesOrders();
        },
        error: (error) => {
          this.errorMessage = 'Failed to delete sales order.';
          this.error.set(error);
        },
      });
    }
  }

  openModal(salesOrder?: SalesOrder) {
    this.salesOrderForm.reset();
    this.items.clear();

    if (salesOrder) {
      this.salesOrderForm.get('salesOrder')?.setValue({
        id: salesOrder.id,
        orderNumber: salesOrder.orderNumber,
        customerId: salesOrder.customerId,
        orderDate: salesOrder.orderDate,
        isShipped: salesOrder.isShipped,
        discount: salesOrder.discount,
        tax: salesOrder.tax,
        totalAmount: salesOrder.totalAmount,
      });

      salesOrder.items?.forEach((item) => {
        this.items.push(
          this.fb.group({
            productId: [item.productId, Validators.required],
            quantity: [item.quantity, [Validators.required, Validators.min(1)]],
            unitPrice: [item.unitPrice, [Validators.required, Validators.min(0)]],
            warehouseId: [item.warehouseId, Validators.required],
            status: [item.status],
          })
        );
      });
    }
    this.showModal.set(true);
  }

  closeModal() {
    this.showModal.set(false);
    this.error.set(null);
  }
}
