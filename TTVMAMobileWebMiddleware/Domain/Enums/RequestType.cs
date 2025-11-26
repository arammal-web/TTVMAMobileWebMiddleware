namespace TTVMAMobileWebMiddleware.Domain.Enums
{
    /// <summary>Types of requests in the workflow.</summary>
    public enum RequestType
    {
        /// <summary>Enrollment Request</summary>
        EnrollmentRequest = 1,

        /// <summary>Operation Request</summary>
        OperationRequest = 2,

        /// <summary>Printing Request</summary>
        PrintingRequest = 3,

        /// <summary>Quality Control Request</summary>
        QualityControlRequest = 4,

        /// <summary>DL Exam Request</summary>
        DLExamRequest = 5 
    }
}
