namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// Response after application approval
/// </summary>
public class ApplicationApprovalResponse
{
    public string ApplicationId { get; set; }
    public int? ReceiptId { get; set; }
    public string? ReceiptNumber { get; set; }
    public bool IsPayable { get; set; }
    public string Status { get; set; } = null!;
    public string ApprovalStatus { get; set; } = null!;
    public string Message { get; set; } = null!;
}

