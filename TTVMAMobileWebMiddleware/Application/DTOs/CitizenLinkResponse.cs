namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// Response after linking or creating a citizen
/// </summary>
public class CitizenLinkResponse
{
    public int CitizenOnlineId { get; set; }
    public int CitizenLocalId { get; set; }
    public string Status { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DrivingLicenseSnapshot? DrivingLicenseSnapshot { get; set; }
}

/// <summary>
/// Driving license snapshot information
/// </summary>
public class DrivingLicenseSnapshot
{
    public string? LicenseNumber { get; set; }
    public List<string> Categories { get; set; } = new();
    public DateTime? IssuanceDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public int? StructureId { get; set; }
    public string? StructureName { get; set; }
}

