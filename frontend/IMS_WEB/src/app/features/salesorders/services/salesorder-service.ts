import { Injectable } from '@angular/core';
import { ApiService } from '../../../shared/api.service';
import { ApiResponse } from '../../../models/apiResponse';
import {
  SalesOrder,
  SalesOrderCreate,
  SalesOrderWithItems,
} from '../../../models/salesorders/salesorder';

@Injectable({
  providedIn: 'root',
})
export class SalesorderService {
  constructor(private apiService: ApiService) {}

  getSalesOrders(customerId: number) {
    return this.apiService.get<ApiResponse<SalesOrder[]>>('SalesOrders/customer/' + customerId);
  }

  getAllSalesOrders() {
    return this.apiService.get<ApiResponse<SalesOrder[]>>('SalesOrders');
  }

  createSalesOrder(salesOrder: SalesOrderWithItems) {
    return this.apiService.post<ApiResponse<SalesOrderWithItems>>('SalesOrders', salesOrder);
  }

  updateSalesOrder(id: number, salesOrder: SalesOrderWithItems) {
    return this.apiService.put<ApiResponse<SalesOrderWithItems>>('SalesOrders', id, salesOrder);
  }

  deleteSalesOrder(id: number) {
    return this.apiService.delete<ApiResponse<void>>('SalesOrders', id);
  }
}
