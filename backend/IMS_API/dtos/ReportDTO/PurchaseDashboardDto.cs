namespace IMS_API.dtos.ReportDTO;

public class PurchaseDashboardDto
{
    public decimal TotalPurchaseAmount { get; set; }
    public int TotalOrdersCount { get; set; }
    public int PendingOrdersCount { get; set; }
    public int ReceivedOrdersCount { get; set; }
    public decimal AverageOrderValue { get; set; }
    public List<PurchaseBySupplierDto> SupplierPurchases { get; set; } = new();
    public List<PendingPurchaseDto> PendingOrders { get; set; } = new();
}

public class PurchaseBySupplierDto
{
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = null!;
    public int OrderCount { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime LastOrderDate { get; set; }
}

public class PendingPurchaseDto
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = null!;
    public string SupplierName { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int ItemCount { get; set; }
}
