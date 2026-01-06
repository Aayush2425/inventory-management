namespace IMS_API.dtos.ReportDTO;

public class SalesDashboardDto
{
    public decimal TotalRevenue { get; set; }
    public int TotalOrdersCount { get; set; }
    public int PendingShipments { get; set; }
    public int CompletedShipments { get; set; }
    public decimal AverageOrderValue { get; set; }
    public List<DailySalesDto> DailySales { get; set; } = new();
    public List<SalesByProductDto> TopProducts { get; set; } = new();
}

public class DailySalesDto
{
    public DateTime Date { get; set; }
    public decimal Revenue { get; set; }
    public int OrderCount { get; set; }
}

public class SalesByProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int QuantitySold { get; set; }
    public decimal TotalRevenue { get; set; }
    public int OrderCount { get; set; }
}
