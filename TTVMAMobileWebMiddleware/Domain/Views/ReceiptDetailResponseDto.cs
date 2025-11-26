namespace TTVMAMobileWebMiddleware.Domain.Views
{
    public sealed class ReceiptDetailResponseDto
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; }
        public int? ItemId { get; set; }
        public int ProcessId { get; set; }
        public int BPVarietyId { get; set; }
        public string? ItemDescriptionAR { get; set; }
        public string? ItemDescriptionEN { get; set; }
        public string? ItemCode { get; set; }
        public int? ItemTypeId { get; set; }
        public int? ItemCategoryId { get; set; }
        public string? ItemCategoryEn { get; set; }
        public string? ItemCategoryAr { get; set; }
        public string? ItemCategoryFr { get; set; }
        public float Amount { get; set; }
        public string? Notes { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedUserId { get; set; }
    }
}
