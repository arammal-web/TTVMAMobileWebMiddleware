namespace TTVMAMobileWebMiddleware.Domain.Views

{

    public sealed class ApplicationProcessFeeDto
    {
        public int FeeId { get; set; }
        public decimal? FeeValue { get; set; }
        public decimal? FeeTax { get; set; }
        public bool? IsPaid { get; set; }

        // Fee multilingual (kept on APP.ApplicationProcessFee)
        public string FeeNameEn { get; set; } = null!;
        public string FeeNameAr { get; set; } = null!;
        public string FeeNameFr { get; set; } = null!;
    }
}
