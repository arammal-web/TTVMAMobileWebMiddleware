using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.RequestUtility;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TTVMAMobileWebMiddleware.Application.DTOs;
using TTVMAMobileWebMiddleware.Application.Interfaces;

namespace TTVMAMobileWebMiddleware.API.Controllers;

/// <summary>
/// API controller for application approval operations
/// </summary>
[ApiController]
//[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/applications")]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly ILogger<ApplicationsController> _logger;

    public ApplicationsController(
        IApplicationService applicationService,
        ILogger<ApplicationsController> logger)
    {
        _applicationService = applicationService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all application records.
    /// </summary> 
    /// <remarks>Returns a list of all applications.</remarks>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
    {
        //get all pending applications from Mobile Database 
        var (items, metaData) = await _applicationService.GetAllAsync(pagination);
        Response.Headers.Add("Pagination-MetaData", System.Text.Json.JsonSerializer.Serialize(metaData));
        return Ok(items);
    }

    /// <summary>
    /// Retrieves an application by its ID.
    /// </summary>
    /// <param name="id">The ID of the application to retrieve.</param>
    /// <returns>The application record if found; otherwise, 404 Not Found.</returns>
    /// <remarks>Retrieves an application by its ID.</remarks>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _applicationService.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }
    /// <summary>
    /// Retrieves an application by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("details/{id}")]
    public async Task<IActionResult> GetByIdWithDetails(string id)
    {
        var result = await _applicationService.GetByIdWithDetailAsync(id);
        return result == null ? NotFound() : Ok(result);
    }
    /// <summary>
    /// Reviews and approves an application, creating records in local DLS
    /// </summary>
    /// <param name="request">Application review request with documents and fees</param>
    /// <returns>Application approval response with receipt information</returns>
    [HttpPost("review")]
    public async Task<IActionResult> ReviewAndApprove([FromBody] string applicationId)
    {
        try
        {
            var result = await _applicationService.ApprovePendingApplication(applicationId);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation during application review");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reviewing application {ApplicationId}", applicationId);
            return StatusCode(500, new { error = "An error occurred while processing the application review" });
        }
    }


    /// <summary>
    /// Retrieves an application by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("reject/{id}")]
    public async Task<IActionResult> RejectApplication(string id)
    {
        var result = await _applicationService.RejectApplication(id);
        return result == null ? NotFound() : Ok(result);
    }


    /// <summary>
    /// Retrieves an application by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("docrequired/{id}")]
    public async Task<IActionResult> DocumentRequiredApplication(string id)
    {
        var result = await _applicationService.DocumentRequiredApplication(id);
        return result == null ? NotFound() : Ok(result);
    }


    [HttpGet("GetApplications")]
    public async Task<IActionResult> GetApplications([FromQuery] Pagination pagination, [FromQuery] string? keyword = null, [FromQuery] string? status = null, [FromQuery] string? filtration = "all", [FromQuery] int? userId = null, [FromQuery] int? branchId = null, CancellationToken ct = default)
    {
        // Validate filtration parameter
        if (!string.IsNullOrWhiteSpace(filtration) &&
            !filtration.Equals("all", StringComparison.OrdinalIgnoreCase) &&
            !filtration.Equals("user", StringComparison.OrdinalIgnoreCase) &&
            !filtration.Equals("branch", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Invalid filtration parameter. Valid values are: 'all', 'user', 'branch'");
        }

        // Validate required parameters based on filtration type
        if (filtration.Equals("user", StringComparison.OrdinalIgnoreCase) && !userId.HasValue)
        {
            return BadRequest("userId parameter is required when filtration='user'");
        }

        if (filtration.Equals("branch", StringComparison.OrdinalIgnoreCase) && !branchId.HasValue)
        {
            return BadRequest("branchId parameter is required when filtration='branch'");
        }

        var result = await _applicationService.GetApplicationsAsync(pagination, keyword, status, filtration, userId, branchId, ct);
 
        Response.Headers.Add("Pagination-MetaData", System.Text.Json.JsonSerializer.Serialize(result.metaData));
 
        return Ok(result.items);
    }




}

