using TTVMAMobileWebMiddleware.Domain.Enums;

namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// Request to link online citizen to local citizen
/// </summary>
public class CitizenLinkRequest
{
    public int CitizenOnlineId { get; set; }
    public int CitizenLocalId { get; set; }
    public LinkMethod LinkMethod { get; set; }
    public decimal Confidence { get; set; }
    public string? DecisionNote { get; set; }
}

