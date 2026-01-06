export interface InventorySummaryDto {
  totalInventoryValue: number;
  totalItems: number;
  lowStockCount: number;
  outOfStockCount: number;
  warehouseStocks: WarehouseStockDto[];
  lowStockAlerts: LowStockAlertDto[];
}

export interface WarehouseStockDto {
  warehouseId: number;
  warehouseName: string;
  stockValue: number;
  totalUnits: number;
}

export interface LowStockAlertDto {
  productId: number;
  productName: string;
  sku: string;
  currentStock: number;
  reorderLevel: number;
  warehouseName: string;
}

export interface SalesDashboardDto {
  totalRevenue: number;
  totalOrdersCount: number;
  pendingShipments: number;
  completedShipments: number;
  averageOrderValue: number;
  dailySales: DailySalesDto[];
  topProducts: SalesByProductDto[];
}

export interface DailySalesDto {
  date: string;
  revenue: number;
  orderCount: number;
}

export interface SalesByProductDto {
  productId: number;
  productName: string;
  quantitySold: number;
  totalRevenue: number;
  orderCount: number;
}

export interface PurchaseDashboardDto {
  totalPurchaseAmount: number;
  totalOrdersCount: number;
  pendingOrdersCount: number;
  receivedOrdersCount: number;
  averageOrderValue: number;
  supplierPurchases: PurchaseBySupplierDto[];
  pendingOrders: PendingPurchaseDto[];
}

export interface PurchaseBySupplierDto {
  supplierId: number;
  supplierName: string;
  orderCount: number;
  totalAmount: number;
  lastOrderDate: string;
}

export interface PendingPurchaseDto {
  orderId: number;
  orderNumber: string;
  supplierName: string;
  orderDate: string;
  expectedDeliveryDate: string;
  totalAmount: number;
  itemCount: number;
}

export interface TopProductsDto {
  topByVolume: TopProductByVolumeDto[];
  topByRevenue: TopProductByRevenueDto[];
}

export interface TopProductByVolumeDto {
  productId: number;
  productName: string;
  sku: string;
  totalQuantitySold: number;
  unitPrice: number;
  rank: number;
}

export interface TopProductByRevenueDto {
  productId: number;
  productName: string;
  sku: string;
  totalRevenue: number;
  quantitySold: number;
  unitPrice: number;
  rank: number;
}

export interface RevenueByCategoryDto {
  categories: CategoryRevenueDto[];
  totalRevenue: number;
}

export interface CategoryRevenueDto {
  categoryId: number;
  categoryName: string;
  revenue: number;
  productCount: number;
  quantitySold: number;
  percentage: number;
}
