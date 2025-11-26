using System;
using TTVMAMobileWebMiddleware.Domain.Enums;

namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// Response model for available appointment slots
/// </summary>
public class AppointmentAvailabilitySlotResponse
{
    public DateTime AppointmentDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Room { get; set; }
    public string? Station { get; set; }
    public int AvailableCapacity { get; set; }
    public bool IsAvailable { get; set; }
}

/// <summary>
/// Response model for appointment availability search
/// </summary>
public class AppointmentAvailabilitySearchResponse
{
    public string ApplicationId { get; set; } = null!;
    public AppointmentPurpose Purpose { get; set; }
    public int StructureId { get; set; }
    public string StructureName { get; set; } = null!;
    public List<AppointmentAvailabilitySlotResponse> AvailableSlots { get; set; } = new();
}

