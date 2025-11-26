namespace TTVMAMobileWebMiddleware.Domain.Views
{
    public class ApplicationListItemDto
    {
        public string ApplicationId { get; set; } = null!;
        public string ApplicationNumber { get; set; } = null!;
        public string ReceiptNumber { get; set; } = null!;
        public string? ReceiptCategorySequenceNumber { get; set; }
        public string Applicant { get; set; } = null!;
        public int ApplicantId { get; set; }
        public string ProcessId { get; set; }
        public string BPVarietyId { get; set; }
        public int ApprovalStatusId { get; set; }

        public string ProcessTypeEn { get; set; } = null!;
        public string ServiceTypeEn { get; set; } = null!;
        public string ApprovalStatusEn { get; set; } = null!;
        public string FeesEn { get; set; } = null!;

        public string ProcessTypeFr { get; set; } = null!;
        public string ServiceTypeFr { get; set; } = null!;
        public string ApprovalStatusFr { get; set; } = null!;
        public string FeesFr { get; set; } = null!;

        public string ProcessTypeAr { get; set; } = null!;
        public string ServiceTypeAr { get; set; } = null!;
        public string ApprovalStatusAr { get; set; } = null!;
        public string FeesAr { get; set; } = null!;
        public string? DrivingLicenseNumber { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public DrivingLicenseDTO DrivingLicense { get; set; }
    }

    public class DrivingLicenseDTO
    {
        public string? DrivingLicenseNumber { get; set; }
        public string? DrivingLicenseInternationalNumber { get; set; }

        public DateTime? IssuanceDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? DrivingLicenseStatusId { get; set; }
    }
}
