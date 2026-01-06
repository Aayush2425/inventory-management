namespace IMS_API.dtos.ReportDTO;

public class InventorySummaryDto
{
    public decimal TotalInventoryValue { get; set; }
    public int TotalItems { get; set; }
    public int LowStockCount { get; set; }
    public int OutOfStockCount { get; set; }
    public List<WarehouseStockDto> WarehouseStocks { get; set; } = new();
    public List<LowStockAlertDto> LowStockAlerts { get; set; } = new();
}

public class WarehouseStockDto
{
    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; } = null!;
    public decimal StockValue { get; set; }
    public int TotalUnits { get; set; }
}

public class LowStockAlertDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string? SKU { get; set; }
    public int CurrentStock { get; set; }
    public int ReorderLevel { get; set; }
    public string WarehouseName { get; set; } = null!;
}
