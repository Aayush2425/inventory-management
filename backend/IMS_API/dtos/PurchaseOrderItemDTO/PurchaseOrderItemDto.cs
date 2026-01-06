namespace IMS_API.dtos.PurchaseOrderItemDTO
{
    public class PurchaseOrderItemDto
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public int WarehouseId { get; set; }
        public string? WarehouseName { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
