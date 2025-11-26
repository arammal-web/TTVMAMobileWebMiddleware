using System;
using System.ComponentModel.DataAnnotations;
using TTVMAMobileWebMiddleware.Domain.Enums;

using AppointmentStatus = TTVMAMobileWebMiddleware.Domain.Enums.AppointmentStatus;

namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// Request model for booking an appointment
/// </summary>
public class AppointmentBookRequest
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

    /// <summary>
    /// Hold token from the hold operation (if used)
    /// </summary>
    public string? HoldToken { get; set; }

    /// <summary>
    /// Optional notes
    /// </summary>
    public string? Notes { get; set; }
}

/// <summary>
/// Response model for booking an appointment
/// </summary>
public class AppointmentBookResponse
{
    public int AppointmentId { get; set; }
    public string ApplicationId { get; set; } = null!;
    public AppointmentPurpose Purpose { get; set; }
    public AppointmentStatus Status { get; set; }
    public DateTime AppointmentDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Room { get; set; }
    public string? Station { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
}

