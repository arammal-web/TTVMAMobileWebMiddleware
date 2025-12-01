namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// Result of a sync operation
/// </summary>
public class SyncResult
{
    /// <summary>
    /// Number of receipts synced
    /// </summary>
    public int ReceiptsSynced { get; set; }

    /// <summary>
    /// Number of receipt details synced
    /// </summary>
    public int ReceiptDetailsSynced { get; set; }

    /// <summary>
    /// Number of driving licenses synced
    /// </summary>
    public int DrivingLicensesSynced { get; set; }

    /// <summary>
    /// Number of driving license details synced
    /// </summary>
    public int DrivingLicenseDetailsSynced { get; set; }

    /// <summary>
    /// Number of citizens processed
    /// </summary>
    public int CitizensProcessed { get; set; }

    /// <summary>
    /// Number of applications processed
    /// </summary>
    public int ApplicationsProcessed { get; set; }

    /// <summary>
    /// Any errors encountered during sync
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// Indicates if the sync completed successfully
    /// </summary>
    public bool IsSuccess => Errors.Count == 0;
}

