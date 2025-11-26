namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// Request to approve or reject an online citizen
/// </summary>
public class CitizenApproveRequest
{
    public int CitizenOnlineId { get; set; }
    public bool IsApproved { get; set; }
    public string? RejectionReason { get; set; }
}

