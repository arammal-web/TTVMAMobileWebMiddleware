namespace TTVMAMobileWebMiddleware.Domain.Views
{
    public class ActionMenu
    {
        public bool SetApproved { get; set; }
        public bool SetRejected { get; set; }
        public bool SetCancelled { get; set; }
        public bool SetOnHold { get; set; }
        public bool PrintReceipt { get; set; }
        public bool CreateRequestForEnrollment { get; set; }
        public bool CreateRequestForExam { get; set; }
        public bool CreateRequestForProduction { get; set; }
    }
}
