namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// DTO for citizen list items with status information
/// </summary>
public class CitizenListItemDto
{
    public int Id { get; set; }
    public long UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? NationalId { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int ApprovalStatusId { get; set; }
    public string? ApprovalStatusEn { get; set; }
    public string? ApprovalStatusAr { get; set; }
    public string? ApprovalStatusFr { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

