namespace TTVMAMobileWebMiddleware.Domain.Views
{
    public class PayReceiptResult
    {

        public bool ExamRequired { get; set; }
        public bool UrgentExamRequired { get; set; }
        public bool IsPaid { get; set; }
        public int? DrivingTestRequestId { get; set; }
        public bool DailyRegionLimitReached { get; set; } = false;
        public bool IsAssigned { get; set; } = false;
        public bool AutoCreatedNewList { get; set; } = false;
        public int? ExamListId { get; set; }
        public string? ExamListNumber { get; set; }
        public string? ErrorMessage { get; set; } = string.Empty;
        public string? ErrorCode { get; set; } = string.Empty;

        

    }
}
