export interface Product {
  id: number;
  name: string;
  sku: string;
  categoryId: number;
  categoryName: string;
  price: number;
  description: string;
  isActive: boolean;
  createdAt: Date;
  InventoryItems?: InventoryItem[];
}
export interface ProductWithInventoryCreate {
  product: {
    id: number;
    name: string;
    sku: string;
    categoryId: number;
    price: number;
    description: string;
    isActive: boolean;
    userId: number;
  };
  inventory: {
    warehouseId: number;
    quantity: number;
    reorderLevel: number;
    reorderQuantity: number;
  };
}
export interface productWithInventory {
  product: Product;
  inventoryItems: InventoryItem;
}
export interface InventoryItem {
  id: number;
  productId: number;
  warehouseId: number;
  quantity: number;
  reorderLevel: number;
  reorderQuantity: number;
}
