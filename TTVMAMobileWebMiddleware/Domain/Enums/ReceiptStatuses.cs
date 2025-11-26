namespace TTVMAMobileWebMiddleware.Domain.Enums
{
    public enum ReceiptStatuses
    {
        /// <summary>
        /// Receipt is active and pending processing.
        /// </summary>
        PendingPayment = 4,
        /// <summary>
        /// Receipt is on hold for further review.
        /// </summary>
        OnHold = 107,
        /// <summary>
        /// Receipt has been cancelled.
        /// </summary>
        Completed = 103,
        /// <summary>
        /// Receipt has been cancelled.
        /// </summary>
        Cancelled = 108

    }
}
