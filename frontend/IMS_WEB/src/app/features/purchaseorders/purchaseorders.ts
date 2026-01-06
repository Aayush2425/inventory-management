import { Component, signal } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Sidebar } from '../../shared/components/sidebar/sidebar';
import {
  PurchaseOrder,
  PurchaseOrderCreate,
  PurchaseOrderItem,
  PurchaseOrderWithItems,
} from '../../models/puchaseorders.ts/purchaseorder';
import { Warehouse } from '../../models/warehouses/warehoue';
import { WarehouseService } from '../warehouses/services/warehouse-service';
import { SupplierService } from '../suppliers/services/supplier-service';
import { Supplier } from '../../models/suppilers/supplier';
import { ProductServices } from '../products/services/product-services';
import { Product } from '../../models/products/product';
import { PurchaseorderService } from './services/purchaseorder-service';
@Component({
  selector: 'app-purchaseorders',
  imports: [Sidebar, ReactiveFormsModule, FormsModule],
  templateUrl: './purchaseorders.html',
  styleUrl: './purchaseorders.css',
})
export class Purchaseorders {
  purchaseOrders = signal<PurchaseOrder[]>([]);

  warehouses = signal<Warehouse[]>([]);
  suppliers = signal<Supplier[]>([]);
  products = signal<Product[]>([]);
  purchaseOrderForm!: FormGroup;
  error = signal<string | null>(null);
  showModal = signal(false);
  errorMessage = '';
  successMessage = '';

  searchTerm = signal<string>('');
  warehouseFilter = signal<string>('');
  constructor(
    private fb: FormBuilder,
    private warehouseService: WarehouseService,
    private supplierService: SupplierService,
    private productService: ProductServices,
    private purchaseOrderService: PurchaseorderService
  ) {
    this.purchaseOrderForm = this.fb.group({
      purchaseOrder: this.fb.group({
        id: [0],
        orderNumber: ['', Validators.required],
        supplierId: [0, Validators.required],
        orderDate: [new Date(), Validators.required],
        expectedDeliveryDate: [null],
        isReceived: [false, Validators.required],
        totalAmount: [0, [Validators.required, Validators.min(0)]],
      }),

      inventoryItems: this.fb.array([]),
    });
  }
  ngOnInit(): void {
    this.loadWarehouses();
    this.loadSuppliers();
    this.loadProducts();
  }
  loadWarehouses() {
    this.warehouseService.getWarehouses().subscribe({
      next: (response) => {
        this.warehouses.set(response?.data ?? []);
        this.loadPurchaseOrders();
      },
      error: (error) => {
        this.error.set('Failed to load warehouses.');
      },
    });
  }
  loadSuppliers() {
    this.supplierService.getSuppliers().subscribe({
      next: (response) => {
        this.suppliers.set(response?.data ?? []);
      },
      error: (error) => {
        this.error.set('Failed to load suppliers.');
      },
    });
  }
  loadProducts() {
    this.productService.getProducts().subscribe({
      next: (response) => {
        this.products.set(response?.data ?? []);
        console.log(this.products());
      },
      error: (error) => {
        this.error.set('Failed to load products');
      },
    });
  }
  loadPurchaseOrders() {
    console.log('HEllo');

    this.warehouses().forEach((warehouse) => {
      this.purchaseOrderService.getPurchaseOrders(warehouse.id).subscribe({
        next: (response) => {
          console.log(response.data);

          this.purchaseOrders.set(response.data ?? []);
          console.log(this.purchaseOrders());
        },
        error: (error) => {
          this.error.set('Failed to load purchase orders');
        },
      });
    });
  }
  get items(): FormArray {
    return this.purchaseOrderForm.get('inventoryItems') as FormArray;
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
    console.log('hello');

    if (this.purchaseOrderForm.valid) {
      const purchaseOrderData = this.purchaseOrderForm.value;
      const purchaseOrderId = purchaseOrderData.purchaseOrder.id;

      // Check if it's an update or create
      if (purchaseOrderId && purchaseOrderId !== 0) {
        this.updatePurchaseOrder(purchaseOrderData);
      } else {
        this.createPurchaseOrder(purchaseOrderData);
      }
    }
  }

  createPurchaseOrder(purchaseOrderData: any): void {
    const payload: PurchaseOrderWithItems = {
      purchaseOrder: {
        id: Number(purchaseOrderData.purchaseOrder.id ?? 0),
        orderNumber: purchaseOrderData.purchaseOrder.orderNumber,
        supplierId: Number(purchaseOrderData.purchaseOrder.supplierId),
        orderDate: purchaseOrderData.purchaseOrder.orderDate,
        expectedDeliveryDate: purchaseOrderData.purchaseOrder.expectedDeliveryDate,
        isReceived: purchaseOrderData.purchaseOrder.isReceived === true,
        totalAmount: Number(purchaseOrderData.purchaseOrder.totalAmount),
      },
      inventoryItems: purchaseOrderData.items.map((item: any) => ({
        productId: Number(item.productId),
        quantity: Number(item.quantity),
        unitPrice: Number(item.unitPrice),
        warehouseId: Number(item.warehouseId),
        status: item.status,
      })),
    };

    console.log('FINAL PAYLOAD SENT:', payload);

    this.purchaseOrderService.createPurchaseOrder(payload).subscribe({
      next: (response) => {
        this.successMessage = 'Purchase order created successfully!';
        this.error.set(null);
        this.closeModal();
        this.loadPurchaseOrders();
        console.log(response.data);
      },
      error: (error) => {
        this.errorMessage = 'Failed to create purchase order.';
        this.error.set(error);
      },
    });
  }

  updatePurchaseOrder(purchaseOrderData: any): void {
    const purchaseOrderId = purchaseOrderData.purchaseOrder.id;

    if (!purchaseOrderId) {
      this.error.set('Purchase order ID is required for update.');
      return;
    }
    console.log(purchaseOrderData);

    const payload: PurchaseOrderWithItems = {
      purchaseOrder: {
        id: Number(purchaseOrderId),
        orderNumber: purchaseOrderData.purchaseOrder.orderNumber,
        supplierId: Number(purchaseOrderData.purchaseOrder.supplierId),
        orderDate: purchaseOrderData.purchaseOrder.orderDate,
        expectedDeliveryDate: purchaseOrderData.purchaseOrder.expectedDeliveryDate,
        isReceived: purchaseOrderData.purchaseOrder.isReceived === true,
        totalAmount: Number(purchaseOrderData.purchaseOrder.totalAmount),
      },
      inventoryItems: purchaseOrderData.inventoryItems.map((item: any) => ({
        id: Number(item.id),
        productId: Number(item.productId),
        quantity: Number(item.quantity),
        unitPrice: Number(item.unitPrice),
        warehouseId: Number(item.warehouseId),
        status: item.status,
      })),
    };

    this.purchaseOrderService.updatePurchaseOrder(purchaseOrderId, payload).subscribe({
      next: (response) => {
        this.successMessage = 'Purchase order updated successfully!';
        this.error.set(null);
        this.closeModal();
        this.loadPurchaseOrders();
      },
      error: (error) => {
        this.errorMessage = 'Failed to update purchase order.';
        this.error.set(error);
      },
    });
  }

  deletePurchaseOrder(id: number): void {
    if (confirm('Are you sure you want to delete this purchase order?')) {
      this.purchaseOrderService.deletePurchaseOrder(id).subscribe({
        next: (response) => {
          this.successMessage = 'Purchase order deleted successfully!';
          this.error.set(null);
          this.loadPurchaseOrders();
        },
        error: (error) => {
          this.errorMessage = 'Failed to delete purchase order.';
          this.error.set(error);
        },
      });
    }
  }
  openModal(purchaseOrder?: PurchaseOrder) {
    this.purchaseOrderForm.reset();
    this.items.clear();

    if (purchaseOrder) {
      this.purchaseOrderForm.get('purchaseOrder')?.setValue({
        id: purchaseOrder.id,
        orderNumber: purchaseOrder.orderNumber,
        supplierId: purchaseOrder.supplierId,
        orderDate: purchaseOrder.orderDate,
        expectedDeliveryDate: purchaseOrder.expectedDeliveryDate ?? null,
        isReceived: purchaseOrder.isReceived,
        totalAmount: purchaseOrder.totalAmount,
      });

      // Load each item into the items FormArray
      purchaseOrder.items?.forEach((item) => {
        console.log(item);

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
