using System;
using System.ComponentModel.DataAnnotations;
using TTVMAMobileWebMiddleware.Domain.Enums;

namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// Request model for holding an appointment slot temporarily
/// </summary>
public class AppointmentHoldRequest
{
    /// <summary>
    /// Application ID
    /// </summary>
    [Required]
    public string ApplicationOnlineId { get; set; } = null!;

    /// <summary>
    /// Purpose of the appointment
    /// </summary>
    [Required]
    public AppointmentPurpose Purpose { get; set; }

    /// <summary>
    /// Structure ID
    /// </summary>
    [Required]
    public int StructureId { get; set; }

    /// <summary>
    /// Appointment date
    /// </summary>
    [Required]
    public DateTime AppointmentDate { get; set; }

    /// <summary>
    /// Start time
    /// </summary>
    [Required]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// End time
    /// </summary>
    [Required]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Optional room identifier
    /// </summary>
    public string? Room { get; set; }

    /// <summary>
    /// Optional station identifier
    /// </summary>
    public string? Station { get; set; }
}

/// <summary>
/// Response model for holding an appointment slot
/// </summary>
public class AppointmentHoldResponse
{
    public string HoldToken { get; set; } = null!;
    public DateTime HoldExpiryTime { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
}

