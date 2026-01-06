namespace IMS_API.Models;

public enum PaymentStatus
{
    Pending = 0,
    Paid = 1,
    PartiallyPaid = 2,
    Failed = 3,
    Cancelled = 4
}
