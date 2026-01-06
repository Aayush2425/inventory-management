import { Injectable } from '@angular/core';
import { ApiService } from '../../../shared/api.service';
import { Warehouse } from '../../../models/warehouses/warehoue';
import { ApiResponse } from '../../../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class WarehouseService {
  constructor(private apiService: ApiService) {}
  getWarehouses() {
    return this.apiService.get<ApiResponse<Warehouse[]>>('Warehouses/user');
  }
  createWarehouse(warehouse: any) {
    return this.apiService.post<ApiResponse<Warehouse>>('Warehouses', warehouse);
  }
  updateWarehouse(warehouse: any) {
    return this.apiService.put<ApiResponse<Warehouse>>(`Warehouses`, warehouse.id, warehouse);
  }
  deleteWarehouse(warehouseId: number) {
    return this.apiService.delete<ApiResponse<null>>(`Warehouses`, warehouseId);
  }
}
