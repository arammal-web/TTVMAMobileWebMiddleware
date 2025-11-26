using System.ComponentModel.DataAnnotations;

namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// Request model for confirming an appointment (officer action)
/// </summary>
public class AppointmentConfirmRequest
{
    /// <summary>
    /// Appointment ID to confirm
    /// </summary>
    [Required]
    public int AppointmentId { get; set; }

    /// <summary>
    /// Optional notes from the officer
    /// </summary>
    public string? Notes { get; set; }
}

/// <summary>
/// Response model for confirming an appointment
/// </summary>
public class AppointmentConfirmResponse
{
    public int AppointmentId { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
}

