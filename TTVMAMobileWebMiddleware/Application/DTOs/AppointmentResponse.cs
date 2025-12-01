using System;
using TTVMAMobileWebMiddleware.Domain.Enums;

namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// Response model for appointment details
/// </summary>
public class AppointmentResponse
{
    public int Id { get; set; }
    public string ApplicationId { get; set; } = null!;
    public string ApplicationNumber { get; set; } = string.Empty;
    public string ApplicantName { get; set; } = string.Empty;
    public string ApplicationType { get; set; } = string.Empty;
    public string AppointmentType { get; set; } = string.Empty;
    public AppointmentPurpose Purpose { get; set; }
    public AppointmentStatus Status { get; set; }
    public int? StatusId { get; set; }
    public string? StatusEn { get; set; }
    public string? StatusAr { get; set; }
    public string? StatusFr { get; set; }
    public int StructureId { get; set; }
    public string StructureName { get; set; } = null!;
    public DateTime AppointmentDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Room { get; set; }
    public string? Station { get; set; }
    public string? Notes { get; set; }
    public string? ProposalMessage { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ConfirmedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
}

