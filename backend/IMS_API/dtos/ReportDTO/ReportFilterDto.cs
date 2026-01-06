namespace IMS_API.dtos.ReportDTO;

public class ReportFilterDto
{
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int? CategoryId { get; set; }
    public int? WarehouseId { get; set; }
    public int? SupplierId { get; set; }
    public int? CustomerId { get; set; }
    public int Top { get; set; } = 10;
}
