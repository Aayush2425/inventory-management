using IMS_API.dtos.ReportDTO;
using IMS_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.services;


public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ReportService> _logger;

    public ReportService(ApplicationDbContext context, ILogger<ReportService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<InventorySummaryDto> GetInventorySummaryAsync(int? warehouseId = null)
    {
        try
        {
            _logger.LogInformation($"Fetching inventory summary{(warehouseId.HasValue ? $" for warehouse {warehouseId}" : "")}");

            var query = from ii in _context.InventoryItems
                        join w in _context.Warehouses on ii.WarehouseId equals w.Id
                        join p in _context.Products on ii.ProductId equals p.Id
                        where !warehouseId.HasValue || ii.WarehouseId == warehouseId
                        select new { ii, w, p };

            var inventoryItems = await query.ToListAsync();

            var totalValue = inventoryItems.Sum(x => x.ii.Quantity * x.p.Price);
            var lowStockItems = inventoryItems.Where(x => x.ii.Quantity <= x.ii.ReorderQuantity).ToList();
            var outOfStockItems = inventoryItems.Where(x => x.ii.Quantity == 0).ToList();

            var lowStockAlerts = lowStockItems
                .Select(x => new LowStockAlertDto
                {
                    ProductId = x.p.Id,
                    ProductName = x.p.Name,
                    SKU = x.p.SKU,
                    CurrentStock = x.ii.Quantity,
                    ReorderLevel = x.ii.ReorderQuantity,
                    WarehouseName = x.w.Name
                })
                .ToList();

            var warehouseStocks = inventoryItems
                .GroupBy(x => x.w.Id)
                .Select(g => new WarehouseStockDto
                {
                    WarehouseId = g.Key,
                    WarehouseName = g.First().w.Name,
                    TotalUnits = g.Sum(x => x.ii.Quantity),
                    StockValue = g.Sum(x => x.ii.Quantity * x.p.Price)
                })
                .ToList();

            return new InventorySummaryDto
            {
                TotalInventoryValue = totalValue,
                TotalItems = inventoryItems.Count,
                LowStockCount = lowStockItems.Count,
                OutOfStockCount = outOfStockItems.Count,
                WarehouseStocks = warehouseStocks,
                LowStockAlerts = lowStockAlerts
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching inventory summary: {ex.Message}");
            throw;
        }
    }

    public async Task<SalesDashboardDto> GetSalesDashboardAsync(DateTime? dateFrom = null, DateTime? dateTo = null, int top = 10)
    {
        try
        {
            dateFrom ??= DateTime.UtcNow.AddMonths(-1);
            dateTo ??= DateTime.UtcNow;

            var salesOrders = await _context.SalesOrders
                .Where(so => so.CreatedAt >= dateFrom && so.CreatedAt <= dateTo)
                .Include(so => so.Items)
                .ThenInclude(i => i.Product)
                .ToListAsync();

            var totalRevenue = salesOrders.Sum(so => so.TotalAmount);
            var totalOrders = salesOrders.Count;
            var completedShipments = salesOrders.Count(so => so.IsShipped);
            var pendingShipments = totalOrders - completedShipments;
            var avgOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;

            var dailySales = salesOrders
                .GroupBy(so => so.CreatedAt.Date)
                .OrderByDescending(g => g.Key)
                .Take(30)
                .Select(g => new DailySalesDto
                {
                    Date = g.Key,
                    Revenue = g.Sum(so => so.TotalAmount),
                    OrderCount = g.Count()
                })
                .ToList();

            var topProducts = salesOrders
                .SelectMany(so => so.Items)
                .GroupBy(soi => new { soi.Product.Id, soi.Product.Name })
                .OrderByDescending(g => g.Sum(soi => (decimal)soi.Quantity * soi.UnitPrice))
                .Take(top)
                .Select(g => new SalesByProductDto
                {
                    ProductId = g.Key.Id,
                    ProductName = g.Key.Name,
                    QuantitySold = g.Sum(soi => soi.Quantity),
                    TotalRevenue = g.Sum(soi => (decimal)soi.Quantity * soi.UnitPrice),
                    OrderCount = g.Select(soi => soi.SalesOrderId).Distinct().Count()
                })
                .ToList();

            return new SalesDashboardDto
            {
                TotalRevenue = totalRevenue,
                TotalOrdersCount = totalOrders,
                CompletedShipments = completedShipments,
                PendingShipments = pendingShipments,
                AverageOrderValue = avgOrderValue,
                DailySales = dailySales,
                TopProducts = topProducts
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching sales dashboard: {ex.Message}");
            throw;
        }
    }

    public async Task<PurchaseDashboardDto> GetPurchaseDashboardAsync(DateTime? dateFrom = null, DateTime? dateTo = null)
    {
        try
        {
            dateFrom ??= DateTime.UtcNow.AddMonths(-1);
            dateTo ??= DateTime.UtcNow;

            var purchaseOrders = await _context.PurchaseOrders
                .Where(po => po.CreatedAt >= dateFrom && po.CreatedAt <= dateTo)
                .Include(po => po.Supplier)
                .Include(po => po.Items)
                .ToListAsync();

            var totalPurchases = purchaseOrders.Sum(po => po.TotalAmount);
            var totalOrders = purchaseOrders.Count;
            var pendingOrdersCount = purchaseOrders.Count(po => !po.IsReceived);
            var receivedOrdersCount = purchaseOrders.Count(po => po.IsReceived);
            var avgOrderValue = totalOrders > 0 ? totalPurchases / totalOrders : 0;

            var supplierPurchases = purchaseOrders
                .GroupBy(po => new { po.Supplier.Id, po.Supplier.Name })
                .Select(g => new PurchaseBySupplierDto
                {
                    SupplierId = g.Key.Id,
                    SupplierName = g.Key.Name,
                    OrderCount = g.Count(),
                    TotalAmount = g.Sum(po => po.TotalAmount),
                    LastOrderDate = g.Max(po => po.CreatedAt)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToList();

            var pendingPurchases = purchaseOrders
                .Where(po => !po.IsReceived)
                .Select(po => new PendingPurchaseDto
                {
                    OrderId = po.Id,
                    OrderNumber = po.OrderNumber,
                    SupplierName = po.Supplier.Name,
                    OrderDate = po.CreatedAt,
                    ExpectedDeliveryDate = po.ExpectedDeliveryDate,
                    TotalAmount = po.TotalAmount,
                    ItemCount = po.Items.Count
                })
                .OrderBy(x => x.ExpectedDeliveryDate)
                .ToList();

            return new PurchaseDashboardDto
            {
                TotalPurchaseAmount = totalPurchases,
                TotalOrdersCount = totalOrders,
                PendingOrdersCount = pendingOrdersCount,
                ReceivedOrdersCount = receivedOrdersCount,
                AverageOrderValue = avgOrderValue,
                SupplierPurchases = supplierPurchases,
                PendingOrders = pendingPurchases
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching purchase dashboard: {ex.Message}");
            throw;
        }
    }

    public async Task<TopProductsDto> GetTopProductsAsync(DateTime? dateFrom = null, DateTime? dateTo = null, int top = 10)
    {
        try
        {
            dateFrom ??= DateTime.UtcNow.AddMonths(-1);
            dateTo ??= DateTime.UtcNow;

            var salesItems = await _context.SalesOrderItems
                .Where(soi => soi.SalesOrder.CreatedAt >= dateFrom && soi.SalesOrder.CreatedAt <= dateTo)
                .Include(soi => soi.Product)
                .ToListAsync();

            var topByVolume = salesItems
                .GroupBy(soi => new { soi.Product.Id, soi.Product.Name, soi.Product.SKU, soi.Product.Price })
                .OrderByDescending(g => g.Sum(soi => soi.Quantity))
                .Take(top)
                .Select((g, index) => new TopProductByVolumeDto
                {
                    ProductId = g.Key.Id,
                    ProductName = g.Key.Name,
                    SKU = g.Key.SKU,
                    TotalQuantitySold = g.Sum(soi => soi.Quantity),
                    UnitPrice = g.Key.Price,
                    Rank = index + 1
                })
                .ToList();

            var topByRevenue = salesItems
                .GroupBy(soi => new { soi.Product.Id, soi.Product.Name, soi.Product.SKU, soi.Product.Price })
                .OrderByDescending(g => g.Sum(soi => (decimal)soi.Quantity * soi.UnitPrice))
                .Take(top)
                .Select((g, index) => new TopProductByRevenueDto
                {
                    ProductId = g.Key.Id,
                    ProductName = g.Key.Name,
                    SKU = g.Key.SKU,
                    TotalRevenue = g.Sum(soi => (decimal)soi.Quantity * soi.UnitPrice),
                    QuantitySold = g.Sum(soi => soi.Quantity),
                    UnitPrice = g.Key.Price,
                    Rank = index + 1
                })
                .ToList();

            return new TopProductsDto
            {
                TopByVolume = topByVolume,
                TopByRevenue = topByRevenue
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching top products: {ex.Message}");
            throw;
        }
    }

    public async Task<RevenueByCategoryDto> GetRevenueByCategoryAsync(DateTime? dateFrom = null, DateTime? dateTo = null)
    {
        try
        {
            dateFrom ??= DateTime.UtcNow.AddMonths(-1);
            dateTo ??= DateTime.UtcNow;

            var salesItems = await _context.SalesOrderItems
                .Where(soi => soi.SalesOrder.CreatedAt >= dateFrom && soi.SalesOrder.CreatedAt <= dateTo)
                .Include(soi => soi.Product)
                .ThenInclude(p => p.Category)
                .ToListAsync();

            var totalRevenue = salesItems.Sum(soi => (decimal)soi.Quantity * soi.UnitPrice);

            var categoryRevenues = salesItems
                .GroupBy(soi => new { soi.Product.Category!.Id, soi.Product.Category.Name })
                .Select(g => new CategoryRevenueDto
                {
                    CategoryId = g.Key.Id,
                    CategoryName = g.Key.Name,
                    Revenue = g.Sum(soi => (decimal)soi.Quantity * soi.UnitPrice),
                    ProductCount = g.Select(soi => soi.Product.Id).Distinct().Count(),
                    QuantitySold = g.Sum(soi => soi.Quantity),
                    Percentage = totalRevenue > 0 ? Math.Round((g.Sum(soi => (decimal)soi.Quantity * soi.UnitPrice) / totalRevenue) * 100, 2) : 0
                })
                .OrderByDescending(x => x.Revenue)
                .ToList();

            return new RevenueByCategoryDto
            {
                TotalRevenue = totalRevenue,
                Categories = categoryRevenues
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching revenue by category: {ex.Message}");
            throw;
        }
    }
}
