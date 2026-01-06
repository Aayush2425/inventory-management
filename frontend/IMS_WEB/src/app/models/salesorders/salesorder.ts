export interface SalesOrder {
  id: number;
  orderNumber: string;

  customerId: number;
  customerName?: string;

  orderDate: Date;

  isShipped: boolean;
  discount: number;
  tax: number;
  totalAmount: number;

  createdAt?: Date;
  updatedAt?: Date | null;

  items?: SalesOrderItem[];
}

export interface SalesOrderCreate {
  salesOrder: {
    orderNumber: string;
    customerId: number;
    orderDate: Date;
    isShipped: boolean;
    discount: number;
    tax: number;
    totalAmount: number;
  };
  items: {
    productId: number;
    quantity: number;
    unitPrice: number;
    warehouseId: number;
    status: string;
  }[];
}

export interface SalesOrderWithItems {
  salesOrder: SalesOrder;
  items: SalesOrderItem[];
}

export interface SalesOrderItem {
  id: number;
  salesOrderId: number;

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
