namespace IMS_API.dtos.ReportDTO;

public class TopProductsDto
{
    public List<TopProductByVolumeDto> TopByVolume { get; set; } = new();
    public List<TopProductByRevenueDto> TopByRevenue { get; set; } = new();
}

public class TopProductByVolumeDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string? SKU { get; set; }
    public int TotalQuantitySold { get; set; }
    public decimal UnitPrice { get; set; }
    public int Rank { get; set; }
}

public class TopProductByRevenueDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string? SKU { get; set; }
    public decimal TotalRevenue { get; set; }
    public int QuantitySold { get; set; }
    public decimal UnitPrice { get; set; }
    public int Rank { get; set; }
}
