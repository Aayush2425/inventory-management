import { Injectable } from '@angular/core';
import { ApiService } from '../../../../shared/api.service';
import { Observable } from 'rxjs';
import {
  InventorySummaryDto,
  PurchaseDashboardDto,
  RevenueByCategoryDto,
  SalesDashboardDto,
  TopProductsDto,
} from '../../../../models/reports/reports';

@Injectable({
  providedIn: 'root',
})
export class OwnerServices {
  constructor(private apiService: ApiService) {}

  getInventorySummary(warehouseId?: number): Observable<InventorySummaryDto> {
    let endpoint = 'Reports/inventory-summary';
    if (warehouseId) {
      endpoint += `?warehouseId=${warehouseId}`;
    }
    return this.apiService.get<InventorySummaryDto>(endpoint);
  }

  getSalesDashboard(
    dateFrom?: string,
    dateTo?: string,
    top: number = 10
  ): Observable<SalesDashboardDto> {
    let endpoint = 'Reports/sales-dashboard';

    // Set default dates if not provided (last 30 days)
    const to = dateTo || new Date().toISOString().split('T')[0];
    const from =
      dateFrom ||
      new Date(new Date().setDate(new Date().getDate() - 30)).toISOString().split('T')[0];

    const params = [`dateFrom=${from}`, `dateTo=${to}`, `top=${top}`];

    endpoint += `?${params.join('&')}`;
    return this.apiService.get<SalesDashboardDto>(endpoint);
  }

  getPurchaseDashboard(dateFrom?: string, dateTo?: string): Observable<PurchaseDashboardDto> {
    let endpoint = 'Reports/purchase-dashboard';

    // Set default dates if not provided (last 30 days)
    const to = dateTo || new Date().toISOString().split('T')[0];
    const from =
      dateFrom ||
      new Date(new Date().setDate(new Date().getDate() - 30)).toISOString().split('T')[0];

    const params = [`dateFrom=${from}`, `dateTo=${to}`];

    endpoint += `?${params.join('&')}`;
    return this.apiService.get<PurchaseDashboardDto>(endpoint);
  }

  getTopProducts(dateFrom?: string, dateTo?: string, top: number = 10): Observable<TopProductsDto> {
    let endpoint = 'Reports/top-products';

    // Set default dates if not provided (last 30 days)
    const to = dateTo || new Date().toISOString().split('T')[0];
    const from =
      dateFrom ||
      new Date(new Date().setDate(new Date().getDate() - 30)).toISOString().split('T')[0];

    const params = [`dateFrom=${from}`, `dateTo=${to}`, `top=${top}`];

    endpoint += `?${params.join('&')}`;
    return this.apiService.get<TopProductsDto>(endpoint);
  }

  getRevenueByCategory(dateFrom?: string, dateTo?: string): Observable<RevenueByCategoryDto> {
    let endpoint = 'Reports/revenue-by-category';

    // Set default dates if not provided (last 30 days)
    const to = dateTo || new Date().toISOString().split('T')[0];
    const from =
      dateFrom ||
      new Date(new Date().setDate(new Date().getDate() - 30)).toISOString().split('T')[0];

    const params = [`dateFrom=${from}`, `dateTo=${to}`];

    endpoint += `?${params.join('&')}`;
    return this.apiService.get<RevenueByCategoryDto>(endpoint);
  }
}
