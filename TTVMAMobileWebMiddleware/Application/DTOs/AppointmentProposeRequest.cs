using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TTVMAMobileWebMiddleware.Domain.Enums;

namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// Request model for proposing alternative appointment slots (officer action)
/// </summary>
public class AppointmentProposeRequest
{
    /// <summary>
    /// Appointment ID to propose alternatives for
    /// </summary>
    [Required]
    public int AppointmentId { get; set; }

    /// <summary>
    /// List of proposed alternative slots
    /// </summary>
    [Required]
    [MinLength(1)]
    public List<ProposedSlot> ProposedSlots { get; set; } = new();

    /// <summary>
    /// Message to the citizen explaining the proposal
    /// </summary>
    [StringLength(2000)]
    public string? Message { get; set; }
}

/// <summary>
/// A proposed alternative slot
/// </summary>
public class ProposedSlot
{
    [Required]
    public DateTime AppointmentDate { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    public string? Room { get; set; }
    public string? Station { get; set; }
}

/// <summary>
/// Response model for proposing alternative slots
/// </summary>
public class AppointmentProposeResponse
{
    public int AppointmentId { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int ProposedSlotsCount { get; set; }
}

