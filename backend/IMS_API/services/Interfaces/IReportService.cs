
using IMS_API.dtos.ReportDTO;
namespace IMS_API.Services.Interfaces;

public interface IReportService
{
    Task<InventorySummaryDto> GetInventorySummaryAsync(int? warehouseId = null);
    Task<SalesDashboardDto> GetSalesDashboardAsync(DateTime? dateFrom = null, DateTime? dateTo = null, int top = 10);
    Task<PurchaseDashboardDto> GetPurchaseDashboardAsync(DateTime? dateFrom = null, DateTime? dateTo = null);
    Task<TopProductsDto> GetTopProductsAsync(DateTime? dateFrom = null, DateTime? dateTo = null, int top = 10);
    Task<RevenueByCategoryDto> GetRevenueByCategoryAsync(DateTime? dateFrom = null, DateTime? dateTo = null);
}