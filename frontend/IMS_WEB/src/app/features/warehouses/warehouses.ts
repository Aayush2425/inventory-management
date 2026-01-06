import { Component, signal } from '@angular/core';
import { Warehouse } from '../../models/warehouses/warehoue';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { WarehouseService } from './services/warehouse-service';
import { Sidebar } from '../../shared/components/sidebar/sidebar';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-warehouses',
  imports: [Sidebar, ReactiveFormsModule, CommonModule],
  templateUrl: './warehouses.html',
  styleUrl: './warehouses.css',
})
export class Warehouses {
  warehouses = signal<Warehouse[]>([]);
  warehouseForm: FormGroup;
  error = signal<string | null>(null);
  showModal = signal(false);
  errorMessage = '';
  successMessage = '';
  constructor(private formBuilder: FormBuilder, private warehouseService: WarehouseService) {
    this.warehouseForm = this.formBuilder.group({
      id: [0],
      name: [''],
      location: [''],
    });
  }
  ngOnInit(): void {
    this.loadWarehouses();
  }
  loadWarehouses() {
    this.warehouseService.getWarehouses().subscribe({
      next: (response) => {
        this.warehouses.set(response?.data ?? []);
      },
    });
  }
  openModal(warehouse?: Warehouse) {
    if (warehouse) {
      this.warehouseForm.setValue({
        id: warehouse.id,
        name: warehouse.name,
        location: warehouse.location,
      });
    } else {
      this.warehouseForm.reset({ id: 0, name: '', location: '' });
    }
    this.showModal.set(true);
  }
  closeModal() {
    this.showModal.set(false);
    this.error.set(null);
  }
  saveWarehouse() {
    if (this.warehouseForm.invalid) {
      this.error.set('Please fill in all required fields.');
      return;
    }
    const warehouseData = this.warehouseForm.value;
    if (warehouseData.id && warehouseData.id > 0) {
      this.warehouseService.updateWarehouse(warehouseData).subscribe({
        next: () => {
          this.successMessage = 'Warehouse updated successfully.';
          this.loadWarehouses();
          this.closeModal();
        },
        error: (err) => {
          this.error.set('Failed to update warehouse. ' + err.message);
        },
      });
    } else {
      this.warehouseService.createWarehouse(warehouseData).subscribe({
        next: () => {
          this.successMessage = 'Warehouse created successfully.';
          this.loadWarehouses();
          this.closeModal();
        },
        error: (err) => {
          this.error.set('Failed to create warehouse. ' + err.message);
        },
      });
    }
  }

  deleteWarehouse(warehouseId: number) {
    this.warehouseService.deleteWarehouse(warehouseId).subscribe({
      next: () => {
        this.successMessage = 'Warehouse deleted successfully.';
        this.loadWarehouses();
      },
      error: (err) => {
        this.error.set('Failed to delete warehouse. ' + err.message);
      },
    });
  }
}
