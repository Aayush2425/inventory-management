using IMS_API.dtos.PurchaseOrderItemDTO;

namespace IMS_API.dtos.PurchaseOrderDTO
{
    public class PurchaseOrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = null!;

        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }

        public bool IsReceived { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<PurchaseOrderItemDto> Items { get; set; } = new();
    }
}
