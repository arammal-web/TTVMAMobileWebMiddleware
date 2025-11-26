namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// Request model for citizen search
/// </summary>
public class CitizenSearchRequest
{
    public string? NationalId { get; set; }
    public string? PassportNumber { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? FirstNameAr { get; set; }
    public string? FatherNameAr { get; set; }
    public string? LastNameAr { get; set; }
    public string? MotherNameAr { get; set; }
    public string? FirstNameEn { get; set; }
    public string? LastNameEn { get; set; }
    public string DateOfBirth { get; set; }
    public string? Mobile { get; set; }
}

