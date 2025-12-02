using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TTVMAMobileWebMiddleware.Application.Interfaces;

namespace TTVMAMobileWebMiddleware.API.Controllers;

/// <summary>
/// API controller for status lookup operations
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/status")]
public class StatusController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly ICitizenService _citizenService;
    private readonly IAppointmentService _appointmentService;

    /// <summary>
    /// Constructor for the StatusController class.
    /// </summary>
    /// <param name="applicationService"></param>
    /// <param name="citizenService"></param>
    /// <param name="appointmentService"></param>
    public StatusController(
        IApplicationService applicationService,
        ICitizenService citizenService,
        IAppointmentService appointmentService)
    {
        _applicationService = applicationService;
        _citizenService = citizenService;
        _appointmentService = appointmentService;
    }

    /// <summary>
    /// Retrieves all citizen status records.
    /// </summary>
    /// <returns>List of citizen statuses</returns>
    [HttpGet("citizen-status")]
    public async Task<IActionResult> GetCitizenStatus()
    {
        var result = await _citizenService.GetCitizenStatusesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves all agenda status records.
    /// </summary>
    /// <returns>List of agenda statuses</returns>
    [HttpGet("agenda-status")]
    public async Task<IActionResult> GetAgendaStatus()
    {
        var result = await _appointmentService.GetAgendaStatusesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves all status records.
    /// </summary>
    /// <returns>List of statuses</returns>
    [HttpGet("application-status")]
    public async Task<IActionResult> GetStatus()
    {
        var result = await _applicationService.GetStatusesAsync();
        return Ok(result);
    }
}

