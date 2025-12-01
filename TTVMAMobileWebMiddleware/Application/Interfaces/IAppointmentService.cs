using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTVMAMobileWebMiddleware.Application.DTOs;

namespace TTVMAMobileWebMiddleware.Application.Interfaces;

/// <summary>
/// Service interface for appointment management operations
/// </summary>
public interface IAppointmentService
{
    /// <summary>
    /// Searches for available appointment slots
    /// </summary>
    Task<AppointmentAvailabilitySearchResponse> SearchAvailabilityAsync(
        AppointmentAvailabilitySearchRequest request,
        CancellationToken ct = default);

    /// <summary>
    /// Holds an appointment slot temporarily (5 minutes) to prevent race conditions
    /// </summary>
    Task<AppointmentHoldResponse> HoldSlotAsync(
        AppointmentHoldRequest request,
        CancellationToken ct = default);

    /// <summary>
    /// Books an appointment (creates with Status=Pending, routes to officer for confirmation)
    /// </summary>
    Task<AppointmentBookResponse> BookAppointmentAsync(
        AppointmentBookRequest request,
        CancellationToken ct = default);

    /// <summary>
    /// Confirms an appointment (officer action) - sets Status=Confirmed and marks requests Ready
    /// </summary>
    Task<AppointmentConfirmResponse> ConfirmAppointmentAsync(
        AppointmentConfirmRequest request,
        int officerUserId,
        CancellationToken ct = default);

    /// <summary>
    /// Proposes alternative appointment slots (officer action) - sets Status=Proposed
    /// </summary>
    Task<AppointmentProposeResponse> ProposeAlternativesAsync(
        AppointmentProposeRequest request,
        int officerUserId,
        CancellationToken ct = default);

    /// <summary>
    /// Gets appointment details by ID
    /// </summary>
    Task<AppointmentResponse?> GetAppointmentByIdAsync(
        int appointmentId,
        CancellationToken ct = default);

    /// <summary>
    /// Gets all appointments for an application
    /// </summary>
    Task<List<AppointmentResponse>> GetAppointmentsByApplicationIdAsync(
        string applicationId,
        CancellationToken ct = default);

    /// <summary>
    /// Marks an appointment as completed (on-site operator action)
    /// </summary>
    Task<bool> MarkAppointmentDoneAsync(
        int appointmentId,
        int operatorUserId,
        CancellationToken ct = default);

    /// <summary>
    /// Cancels an appointment
    /// </summary>
    Task<bool> CancelAppointmentAsync(
        int appointmentId,
        int userId,
        string? reason,
        CancellationToken ct = default);
    Task<List<AppointmentResponse>?> GetAppointments(int? status = null, CancellationToken ct = default);
}

