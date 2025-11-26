namespace TTVMAMobileWebMiddleware.Domain.Views

{
    public sealed class ApplicationProcessDto
    {
        public string ApplicationId { get; set; } = null!;
        public int ProcessId { get; set; }
        public int BPVarietyId { get; set; }

        public ProcessNamesDto? Process { get; set; }
        public ProcessVarietyNamesDto? ProcessVariety { get; set; }

        public List<ApplicationProcessCheckListDto> CheckLists { get; set; } = new();
        public List<ApplicationProcessDocumentDto> Documents { get; set; } = new();
        public List<ApplicationProcessFeeDto> Fees { get; set; } = new();
    }
}
