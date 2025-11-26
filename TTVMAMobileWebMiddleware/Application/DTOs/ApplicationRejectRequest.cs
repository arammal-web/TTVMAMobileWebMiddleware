namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// Request to reject an application
/// </summary>
public class ApplicationRejectRequest
{
    public int ApplicationOnlineId { get; set; }
    public string RejectionReason { get; set; } = null!;
}

