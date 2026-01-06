import { Injectable } from '@angular/core';
import { ApiService } from '../../../shared/api.service';
import { ApiResponse } from '../../../models/apiResponse';
import {
  PurchaseOrder,
  PurchaseOrderCreate,
  PurchaseOrderWithItems,
} from '../../../models/puchaseorders.ts/purchaseorder';

@Injectable({
  providedIn: 'root',
})
export class PurchaseorderService {
  constructor(private apiService: ApiService) {}

  getPurchaseOrders(warehouseId: number) {
    return this.apiService.get<ApiResponse<PurchaseOrder[]>>(
      'PurchaseOrders/warehouse/' + warehouseId
    );
  }

  createPurchaseOrder(purchaseOrder: PurchaseOrderWithItems) {
    return this.apiService.post<ApiResponse<PurchaseOrderWithItems>>(
      'PurchaseOrders',
      purchaseOrder
    );
  }

  updatePurchaseOrder(id: number, purchaseOrder: PurchaseOrderWithItems) {
    console.log(purchaseOrder);

    return this.apiService.put<ApiResponse<PurchaseOrderWithItems>>(
      'PurchaseOrders',
      id,
      purchaseOrder
    );
  }

  deletePurchaseOrder(id: number) {
    return this.apiService.delete<ApiResponse<void>>('PurchaseOrders', id);
  }
}
