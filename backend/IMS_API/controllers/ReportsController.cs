using IMS_API.dtos.ReportDTO;
using IMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS_API.controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }


    [HttpGet("inventory-summary")]
    public async Task<ActionResult<InventorySummaryDto>> GetInventorySummary([FromQuery] int? warehouseId = null)
    {
        try
        {
            _logger.LogInformation($"Getting inventory summary{(warehouseId.HasValue ? $" for warehouse {warehouseId}" : "")}");
            var result = await _reportService.GetInventorySummaryAsync(warehouseId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetInventorySummary: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving inventory summary", error = ex.Message });
        }
    }

    [HttpGet("sales-dashboard")]
    public async Task<ActionResult<SalesDashboardDto>> GetSalesDashboard(
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null,
        [FromQuery] int top = 10)
    {
        try
        {
            _logger.LogInformation($"Getting sales dashboard from {dateFrom:yyyy-MM-dd} to {dateTo:yyyy-MM-dd}");
            var result = await _reportService.GetSalesDashboardAsync(dateFrom, dateTo, top);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetSalesDashboard: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving sales dashboard", error = ex.Message });
        }
    }


    [HttpGet("purchase-dashboard")]
    public async Task<ActionResult<PurchaseDashboardDto>> GetPurchaseDashboard(
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null)
    {
        try
        {
            _logger.LogInformation($"Getting purchase dashboard from {dateFrom:yyyy-MM-dd} to {dateTo:yyyy-MM-dd}");
            var result = await _reportService.GetPurchaseDashboardAsync(dateFrom, dateTo);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetPurchaseDashboard: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving purchase dashboard", error = ex.Message });
        }
    }


    [HttpGet("top-products")]
    public async Task<ActionResult<TopProductsDto>> GetTopProducts(
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null,
        [FromQuery] int top = 10)
    {
        try
        {
            _logger.LogInformation($"Getting top {top} products from {dateFrom:yyyy-MM-dd} to {dateTo:yyyy-MM-dd}");
            var result = await _reportService.GetTopProductsAsync(dateFrom, dateTo, top);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetTopProducts: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving top products", error = ex.Message });
        }
    }

    [HttpGet("revenue-by-category")]
    public async Task<ActionResult<RevenueByCategoryDto>> GetRevenueByCategory(
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null)
    {
        try
        {
            _logger.LogInformation($"Getting revenue by category from {dateFrom:yyyy-MM-dd} to {dateTo:yyyy-MM-dd}");
            var result = await _reportService.GetRevenueByCategoryAsync(dateFrom, dateTo);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in GetRevenueByCategory: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving revenue by category", error = ex.Message });
        }
    }
}
