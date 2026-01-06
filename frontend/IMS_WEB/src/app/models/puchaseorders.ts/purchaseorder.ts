export interface PurchaseOrder {
  id: number;
  orderNumber: string;

  supplierId: number;
  supplierName?: string;

  orderDate: Date;
  expectedDeliveryDate?: Date | null;

  isReceived: boolean;
  totalAmount: number;

  createdAt?: Date;
  updatedAt?: Date | null;

  items?: PurchaseOrderItem[];
}

export interface PurchaseOrderCreate {
  purchaseOrder: {
    orderNumber: string;
    supplierId: number;
    orderDate: Date;
    expectedDeliveryDate?: Date | null;
  };
  items: {
    productId: number;
    quantity: number;
    unitPrice: number;
    warehouseId: number;
  }[];
}

export interface PurchaseOrderWithItems {
  purchaseOrder: PurchaseOrder;
  inventoryItems: PurchaseOrderItem[];
}

export interface PurchaseOrderItem {
  id: number;
  purchaseOrderId: number;

  productId: number;
  productName?: string;

  quantity: number;
  unitPrice: number;

  warehouseId: number;
  warehouseName?: string;

  status: string;

  createdAt: Date;
  updatedAt?: Date | null;
}
