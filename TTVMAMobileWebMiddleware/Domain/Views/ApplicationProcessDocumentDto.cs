namespace TTVMAMobileWebMiddleware.Domain.Views
{
    public sealed class ApplicationProcessDocumentDto
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public string? DocFilePath { get; set; }
        public string? DocFileExt { get; set; }

        // Document multilingual
        public string? DocumentNameEn { get; set; }
        public string? DocumentNameAr { get; set; }
        public string? DocumentNameFr { get; set; }

        // DocumentGroup multilingual
        public string? DocumentGroupEn { get; set; }
        public string? DocumentGroupAr { get; set; }
        public string? DocumentGroupFr { get; set; }
    }
}
