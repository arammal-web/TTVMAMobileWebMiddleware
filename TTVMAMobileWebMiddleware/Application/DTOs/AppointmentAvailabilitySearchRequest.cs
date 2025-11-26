using System;
using System.ComponentModel.DataAnnotations;
using TTVMAMobileWebMiddleware.Domain.Enums;

namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// Request model for searching appointment availability
/// </summary>
public class AppointmentAvailabilitySearchRequest
{
    /// <summary>
    /// Application ID for which the appointment is being requested
    /// </summary>
    [Required]
    public string ApplicationOnlineId { get; set; } = null!;

    /// <summary>
    /// Purpose of the appointment: Enrollment, Production, or Pickup
    /// </summary>
    [Required]
    public AppointmentPurpose Purpose { get; set; }

    /// <summary>
    /// Structure/Branch ID where the appointment should take place
    /// </summary>
    [Required]
    public int StructureId { get; set; }

    /// <summary>
    /// Start date for availability search
    /// </summary>
    [Required]
    public DateTime From { get; set; }

    /// <summary>
    /// End date for availability search
    /// </summary>
    [Required]
    public DateTime To { get; set; }

    /// <summary>
    /// Minimum duration in minutes (default: 15)
    /// </summary>
    public int DurationMin { get; set; } = 15;
}

