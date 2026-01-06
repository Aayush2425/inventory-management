namespace IMS_API.dtos.ReportDTO;

public class RevenueByCategoryDto
{
    public List<CategoryRevenueDto> Categories { get; set; } = new();
    public decimal TotalRevenue { get; set; }
}

public class CategoryRevenueDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public decimal Revenue { get; set; }
    public int ProductCount { get; set; }
    public int QuantitySold { get; set; }
    public decimal Percentage { get; set; }
}
