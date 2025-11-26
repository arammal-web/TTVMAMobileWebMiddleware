using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TTVMAMobileWebMiddleware.Application.DTOs;
using TTVMAMobileWebMiddleware.Application.Interfaces;

namespace TTVMAMobileWebMiddleware.API.Controllers;

/// <summary>
/// API controller for appointment management operations
/// </summary>
[ApiController]
//[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/appointments")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<AppointmentsController> _logger;

    public AppointmentsController(
        IAppointmentService appointmentService,
        ILogger<AppointmentsController> logger)
    {
        _appointmentService = appointmentService;
        _logger = logger;
    }

    /// <summary>
    /// Searches for available appointment slots
    /// </summary>
    /// <param name="request">Availability search request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of available appointment slots</returns>
    [HttpPost("availability/search")]
    public async Task<IActionResult> SearchAvailability(
        [FromBody] AppointmentAvailabilitySearchRequest request,
        CancellationToken ct = default)
    {
        try
        {
            var result = await _appointmentService.SearchAvailabilityAsync(request, ct);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation during availability search");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching appointment availability");
            return StatusCode(500, new { error = "An error occurred while searching availability" });
        }
    }

    /// <summary>
    /// Holds an appointment slot temporarily (5 minutes) to prevent race conditions
    /// </summary>
    /// <param name="request">Hold request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Hold token and expiry time</returns>
    [HttpPost("hold")]
    public async Task<IActionResult> HoldSlot(
        [FromBody] AppointmentHoldRequest request,
        CancellationToken ct = default)
    {
        try
        {
            var result = await _appointmentService.HoldSlotAsync(request, ct);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation during slot hold");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error holding appointment slot");
            return StatusCode(500, new { error = "An error occurred while holding the slot" });
        }
    }

    /// <summary>
    /// Books an appointment (creates with Status=Pending, routes to officer for confirmation)
    /// </summary>
    /// <param name="request">Booking request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Booked appointment details</returns>
    [HttpPost("book")]
    public async Task<IActionResult> BookAppointment(
        [FromBody] AppointmentBookRequest request,
        CancellationToken ct = default)
    {
        try
        {
            var result = await _appointmentService.BookAppointmentAsync(request, ct);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation during appointment booking");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error booking appointment");
            return StatusCode(500, new { error = "An error occurred while booking the appointment" });
        }
    }

    /// <summary>
    /// Confirms an appointment (officer action) - sets Status=Confirmed and marks requests Ready
    /// </summary>
    /// <param name="id">Appointment ID</param>
    /// <param name="request">Confirmation request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Confirmation result</returns>
    [HttpPatch("{id}/confirm")]
    public async Task<IActionResult> ConfirmAppointment(
        int id,
        [FromBody] AppointmentConfirmRequest request,
        CancellationToken ct = default)
    {
        try
        { 
            var result = await _appointmentService.ConfirmAppointmentAsync(request, 1, ct);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation during appointment confirmation");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming appointment {AppointmentId}", id);
            return StatusCode(500, new { error = "An error occurred while confirming the appointment" });
        }
    }

    /// <summary>
    /// Proposes alternative appointment slots (officer action) - sets Status=Proposed
    /// </summary>
    /// <param name="id">Appointment ID</param>
    /// <param name="request">Proposal request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Proposal result</returns>
    [HttpPatch("{id}/propose")]
    public async Task<IActionResult> ProposeAlternatives(
        int id,
        [FromBody] AppointmentProposeRequest request,
        CancellationToken ct = default)
    {
        try
        {
            if (id != request.AppointmentId)
            {
                return BadRequest(new { error = "Appointment ID mismatch" });
            }

            // Get current user ID (officer)
            var officerUserId = GetCurrentUserId();

            var result = await _appointmentService.ProposeAlternativesAsync(request, officerUserId, ct);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation during appointment proposal");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error proposing alternatives for appointment {AppointmentId}", id);
            return StatusCode(500, new { error = "An error occurred while proposing alternatives" });
        }
    }

    /// <summary>
    /// Gets appointment details by ID
    /// </summary>
    /// <param name="id">Appointment ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Appointment details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointmentById(
        int id,
        CancellationToken ct = default)
    {
        try
        {
            var result = await _appointmentService.GetAppointmentByIdAsync(id, ct);
            return result == null ? NotFound() : Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointment {AppointmentId}", id);
            return StatusCode(500, new { error = "An error occurred while retrieving the appointment" });
        }
    }

    /// <summary>
    /// Gets appointment details by ID
    /// </summary>
    /// <param name="id">Appointment ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Appointment details</returns>
    [HttpGet]
    public async Task<IActionResult> GetAppointments(
        
        CancellationToken ct = default)
    {
        try
        {
            var result = await _appointmentService.GetAppointments( ct);
            return result == null ? NotFound() : Ok(result);
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, new { error = "An error occurred while retrieving the appointment" });
        }
    }

    /// <summary>
    /// Gets all appointments for an application
    /// </summary>
    /// <param name="applicationId">Application ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of appointments</returns>
    [HttpGet("application/{applicationId}")]
    public async Task<IActionResult> GetAppointmentsByApplicationId(
        string applicationId,
        CancellationToken ct = default)
    {
        try
        {
            var result = await _appointmentService.GetAppointmentsByApplicationIdAsync(applicationId, ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointments for application {ApplicationId}", applicationId);
            return StatusCode(500, new { error = "An error occurred while retrieving appointments" });
        }
    }

    /// <summary>
    /// Marks an appointment as completed (on-site operator action)
    /// </summary>
    /// <param name="id">Appointment ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Completion result</returns>
    [HttpPatch("{id}/complete")]
    public async Task<IActionResult> MarkAppointmentDone(
        int id,
        CancellationToken ct = default)
    {
        try
        {
            // Get current user ID (operator)
            var operatorUserId = GetCurrentUserId();

            var result = await _appointmentService.MarkAppointmentDoneAsync(id, operatorUserId, ct);
            return Ok(new { success = result, message = "Appointment marked as completed" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation during appointment completion");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing appointment {AppointmentId}", id);
            return StatusCode(500, new { error = "An error occurred while completing the appointment" });
        }
    }

    /// <summary>
    /// Cancels an appointment
    /// </summary>
    /// <param name="id">Appointment ID</param>
    /// <param name="reason">Cancellation reason</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Cancellation result</returns>
    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> CancelAppointment(
        int id,
        [FromBody] string? reason,
        CancellationToken ct = default)
    {
        try
        {
            // Get current user ID
            var userId = GetCurrentUserId();

            var result = await _appointmentService.CancelAppointmentAsync(id, userId, reason, ct);
            return Ok(new { success = result, message = "Appointment cancelled successfully" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation during appointment cancellation");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling appointment {AppointmentId}", id);
            return StatusCode(500, new { error = "An error occurred while cancelling the appointment" });
        }
    }

    /// <summary>
    /// Gets the current user ID from claims
    /// </summary>
    private int GetCurrentUserId()
    {
        // TODO: Implement proper authentication and get user ID from claims
        // For now, return a default value
        var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }

        // Default to 1 for development/testing
        return 1;
    }
}

