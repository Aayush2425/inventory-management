import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Sidebar } from '../../../shared/components/sidebar/sidebar';
import { OwnerServices } from './services/owner-services';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

interface ReportCard {
  title: string;
  icon: string;
  value: string | number;
  trend: number;
  color: string;
  bgColor: string;
  description: string;
}

@Component({
  selector: 'app-owner-dashboard',
  imports: [Sidebar, CommonModule],
  templateUrl: './owner-dashboard.html',
  styleUrl: './owner-dashboard.css',
})
export class OwnerDashboard implements OnInit, OnDestroy {
  reports: ReportCard[] = [];
  loading = true;
  owner_name = 'Owner';
  error: string | null = null;
  private destroy$ = new Subject<void>();

  constructor(private ownerService: OwnerServices, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.loadReports();
  }

  async loadReports(): Promise<void> {
    this.loading = true;
    this.error = null;
    this.reports = [];

    try {
      console.log('Starting to load reports...');

      // Load all report data in parallel with timeout
      const timeoutPromise = new Promise((_, reject) =>
        setTimeout(() => reject(new Error('Reports loading timed out after 30 seconds')), 30000)
      );

      await Promise.race([
        Promise.all([
          this.loadInventorySummary(),
          this.loadSalesDashboard(),
          this.loadPurchaseDashboard(),
          this.loadRevenueByCategory(),
          this.loadTopProducts(),
        ]),
        timeoutPromise,
      ]);

      console.log('All reports loaded successfully');
    } catch (err: any) {
      this.error = err?.message || 'Failed to load reports. Please try again later.';
      console.error('Error loading reports:', err);
    } finally {
      this.loading = false;
      this.cdr.detectChanges();
    }
  }

  private loadInventorySummary(): Promise<void> {
    return new Promise((resolve, reject) => {
      console.log('Loading Inventory Summary...');
      this.ownerService
        .getInventorySummary()
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (data) => {
            console.log('Inventory Summary loaded:', data);
            const totalValue = data.totalInventoryValue.toFixed(2);
            const lowStockTrend = data.lowStockCount > 0 ? -5 : 8;

            this.reports.push({
              title: 'Inventory Summary',
              icon: 'bi-box-seam',
              value: `$${this.formatNumber(parseFloat(totalValue))}`,
              trend: 12,
              color: '#6366f1',
              bgColor: '#e0e7ff',
              description: `${data.totalItems} items across ${data.warehouseStocks.length} warehouses`,
            });
            resolve();
          },
          error: (err) => {
            console.error('Error loading inventory summary:', err);
            reject(err);
          },
        });
    });
  }

  private loadSalesDashboard(): Promise<void> {
    return new Promise((resolve, reject) => {
      console.log('Loading Sales Dashboard...');
      this.ownerService
        .getSalesDashboard()
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (data) => {
            console.log('Sales Dashboard loaded:', data);
            const totalRevenue = data.totalRevenue.toFixed(2);
            const trendPercentage = data.totalOrdersCount > 0 ? 18 : 0;

            this.reports.push({
              title: 'Sales Dashboard',
              icon: 'bi-graph-up',
              value: `$${this.formatNumber(parseFloat(totalRevenue))}`,
              trend: trendPercentage,
              color: '#10b981',
              bgColor: '#d1fae5',
              description: `${data.totalOrdersCount} orders | ${data.completedShipments} completed`,
            });
            resolve();
          },
          error: (err) => {
            console.error('Error loading sales dashboard:', err);
            reject(err);
          },
        });
    });
  }

  private loadPurchaseDashboard(): Promise<void> {
    return new Promise((resolve, reject) => {
      console.log('Loading Purchase Dashboard...');
      this.ownerService
        .getPurchaseDashboard()
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (data) => {
            console.log('Purchase Dashboard loaded:', data);
            const totalAmount = data.totalPurchaseAmount.toFixed(2);
            const pendingTrend = data.pendingOrdersCount > 0 ? -3 : 8;

            this.reports.push({
              title: 'Purchase Summary',
              icon: 'bi-cart-check',
              value: `$${this.formatNumber(parseFloat(totalAmount))}`,
              trend: pendingTrend,
              color: '#f59e0b',
              bgColor: '#fef3c7',
              description: `${data.totalOrdersCount} orders | ${data.receivedOrdersCount} received`,
            });
            resolve();
          },
          error: (err) => {
            console.error('Error loading purchase dashboard:', err);
            reject(err);
          },
        });
    });
  }

  private loadRevenueByCategory(): Promise<void> {
    return new Promise((resolve, reject) => {
      console.log('Loading Revenue by Category...');
      this.ownerService
        .getRevenueByCategory()
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (data) => {
            console.log('Revenue by Category loaded:', data);
            const topCategory =
              data.categories.length > 0 ? data.categories[0].categoryName : 'No Data';
            const categoryCount = data.categories.length;

            this.reports.push({
              title: 'Revenue by Category',
              icon: 'bi-pie-chart',
              value: `${categoryCount} Categories`,
              trend: 15,
              color: '#ef4444',
              bgColor: '#fee2e2',
              description: `Top: ${topCategory} | Total: $${this.formatNumber(data.totalRevenue)}`,
            });
            resolve();
          },
          error: (err) => {
            console.error('Error loading revenue by category:', err);
            reject(err);
          },
        });
    });
  }

  private loadTopProducts(): Promise<void> {
    return new Promise((resolve, reject) => {
      console.log('Loading Top Products...');
      // Combine top products and inventory summary in parallel
      Promise.all([
        new Promise<any>((res, rej) => {
          this.ownerService.getTopProducts().pipe(takeUntil(this.destroy$)).subscribe({
            next: res,
            error: rej,
          });
        }),
        new Promise<any>((res, rej) => {
          this.ownerService.getInventorySummary().pipe(takeUntil(this.destroy$)).subscribe({
            next: res,
            error: rej,
          });
        }),
      ])
        .then(([topProductsData, inventoryData]) => {
          console.log('Top Products loaded:', topProductsData);
          const topProductCount = topProductsData.topByRevenue.length;
          const topProduct =
            topProductsData.topByRevenue.length > 0
              ? topProductsData.topByRevenue[0].productName
              : 'N/A';

          this.reports.push({
            title: 'Top Products',
            icon: 'bi-star',
            value: `${topProductCount} Products`,
            trend: 22,
            color: '#8b5cf6',
            bgColor: '#f3e8ff',
            description: `Best: ${topProduct} by revenue`,
          });

          // Add Low Stock Alerts as 6th card
          this.reports.push({
            title: 'Low Stock Alerts',
            icon: 'bi-exclamation-triangle',
            value: `${inventoryData.lowStockCount}`,
            trend: inventoryData.lowStockCount > 0 ? -5 : 5,
            color: '#ec4899',
            bgColor: '#fce7f3',
            description: 'Products below reorder level',
          });

          resolve();
        })
        .catch((err) => {
          console.error('Error loading top products:', err);
          reject(err);
        });
    });
  }

  getTrendDirection(trend: number): string {
    return trend >= 0 ? 'bi-arrow-up' : 'bi-arrow-down';
  }

  getTrendColor(trend: number): string {
    return trend >= 0 ? 'text-success' : 'text-danger';
  }

  formatNumber(num: number): string {
    if (num >= 1000000) {
      return (num / 1000000).toFixed(2) + 'M';
    } else if (num >= 1000) {
      return (num / 1000).toFixed(2) + 'K';
    }
    return num.toFixed(2);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
