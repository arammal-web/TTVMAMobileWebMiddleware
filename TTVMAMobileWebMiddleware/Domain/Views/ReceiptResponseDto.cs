namespace TTVMAMobileWebMiddleware.Domain.Views
{
    public sealed class ReceiptResponseDto
    {
        public int Id { get; set; }
        public int? FeeId { get; set; }
        public int? ProcessId { get; set; }
        public int? BpVarietyId { get; set; }
        public string? ApplicationId { get; set; }
        public string? ApplicationNumber { get; set; }
        public string? ReceiptNumber { get; set; }
        public string? ReceiptCategorySequenceNumber { get; set; }
        public string? Description { get; set; }
        public int? ReceiptStatusId { get; set; }
        public string? ReceiptStatusEn { get; set; }
        public string? ReceiptStatusAr { get; set; }
        public string? ReceiptStatusFr { get; set; }
        public DateTime? ReceiptStatusDate { get; set; }
        public int? StructureId { get; set; }
        public float? TotalAmount { get; set; }
        public bool? IsPaid { get; set; }
        public DateTime? PaidDate { get; set; }
        public string? PaymentProviderNumber { get; set; }
        public DateTime? PaymentProviderDate { get; set; }
        public string? PaymentProviderData { get; set; }
        public string? DataHash { get; set; }
        public bool? IsPosted { get; set; }
        public DateTime? PostedDate { get; set; }
        public int? PostedUserId { get; set; }
        public string? CitizenFullName { get; set; }
        public int? CitizenId { get; set; }
        public string? Notes { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public string? DrivingLicenseNumber { get; set; }

        // you can keep these as null in the response if not needed
        public object? ApplicationProcessFee { get; set; }
        public object? Fee { get; set; }

        public List<ReceiptDetailResponseDto> ReceiptDetails { get; set; } = new();

    }
}
