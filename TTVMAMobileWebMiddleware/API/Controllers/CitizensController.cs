using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.RequestUtility;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TTVMAMobileWebMiddleware.Application.DTOs;
using TTVMAMobileWebMiddleware.Application.Interfaces;

namespace TTVMAMobileWebMiddleware.API.Controllers;

/// <summary>
/// API controller for citizen search and validation operations
/// </summary>
[ApiController]
//[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/citizens")]
public class CitizensController : ControllerBase
{
    private readonly ICitizenService _citizenService;
    private readonly ILogger<CitizensController> _logger;

    public CitizensController(
         ICitizenService citizenService,
        ILogger<CitizensController> logger)
    {
        _citizenService = citizenService;
        _logger = logger;
    }

    /// <summary>
    /// Searches for citizens in local DLS database
    /// </summary>
    /// <param name="request">Search criteria</param>
    /// <returns>List of candidate matches with scores</returns>
    /// 
    [HttpPost("search-local")]
    public async Task<IActionResult> SearchLocal([FromBody] CitizenSearchRequest request)
    {
        try
        {
            var result = await _citizenService.SearchLocalAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for citizens");
            return StatusCode(500, new { error = "An error occurred while searching for citizens" });
        }
    }

    /// <summary>
    /// GET citizens in mobile database
    /// </summary>
    /// <param name="request">Search criteria</param>
    /// <returns>List of candidate matches with scores</returns>
    /// 
    [HttpGet("search-online")]
    public async Task<IActionResult> SearchOnline([FromQuery] Pagination pagination)
    {
        try
        {
            var (items, meta) = await _citizenService.SearchMobileCitizen(pagination);
            Response.Headers.Add("Pagination-MetaData", System.Text.Json.JsonSerializer.Serialize(meta));

            return Ok(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for citizens");
            return StatusCode(500, new { error = "An error occurred while searching for citizens" });
        }
    }

    /// <summary>
    /// GET citizens in mobile database
    /// </summary>
    /// <param name="request">Search criteria</param>
    /// <returns>List of candidate matches with scores</returns>
    /// 
    [HttpGet("get-online-id")]
    public async Task<IActionResult> GetOnlineCitizenById([FromQuery] int citizenId)
    {
        try
        {
            var result = await _citizenService.GetOnlineCitizenById(citizenId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for citizens");
            return StatusCode(500, new { error = "An error occurred while searching for citizens" });
        }
    }

    /// <summary>
    /// Links an online citizen to an existing local citizen and approves
    /// </summary>
    /// <param name="request">Link request with citizen IDs and method</param>
    /// <returns>Link response with status</returns>
    [HttpPost("link")]
    public async Task<IActionResult> LinkAndApprove([FromBody] CitizenLinkRequest request)
    {
        try
        {
            var result = await _citizenService.LinkAndApproveAsync(request, 0);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation during citizen linking");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error linking citizen {CitizenId}", request.CitizenOnlineId);
            return StatusCode(500, new { error = "An error occurred while linking the citizen" });
        }
    }

    /// <summary>
    /// Creates a new local citizen in DLS and links to online citizen
    /// </summary>
    /// <param name="request">Create request with citizen data</param>
    /// <returns>Link response with new local citizen ID</returns>
    [HttpPost("create-local")]
    public async Task<IActionResult> CreateLocalAndApprove([FromBody] CitizenCreateLocalRequest request)
    {
        try
        {

            var result = await _citizenService.CreateLocalAndApproveAsync(request, 0);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation during citizen creation");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating local citizen for {CitizenId}", request.CitizenOnlineId);
            return StatusCode(500, new { error = "An error occurred while creating the local citizen" });
        }
    }

    /// <summary>
    /// Reviews and merges citizen data for medium-confidence matches
    /// </summary>
    /// <param name="request">Link request with merge decision</param>
    /// <returns>Link response with status</returns>
    [HttpPost("review-merge")]
    public async Task<IActionResult> ReviewAndMerge([FromBody] CitizenLinkRequest request)
    {
        try
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User ID not found in token");

            var result = await _citizenService.ReviewAndMergeAsync(request, userId.Value);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation during citizen merge");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error merging citizen {CitizenId}", request.CitizenOnlineId);
            return StatusCode(500, new { error = "An error occurred while merging the citizen" });
        }
    }

    /// <summary>
    /// Approves or rejects an online citizen
    /// </summary>
    /// <param name="citizenId">Online citizen ID</param>
    /// <param name="request">Approve/reject request</param>
    /// <returns>Success status</returns>
    [HttpPost("{citizenId}/approve")]
    public async Task<IActionResult> ApproveOnlineCitizen(int citizenId, [FromBody] CitizenApproveRequest request)
    {
        try
        {
            if (request.CitizenOnlineId != citizenId)
                return BadRequest(new { error = "Citizen ID mismatch" });

            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User ID not found in token");

            var result = await _citizenService.ApproveOnlineCitizenAsync(request, userId.Value);
            return Ok(new { success = result });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation during citizen approval");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving citizen {CitizenId}", citizenId);
            return StatusCode(500, new { error = "An error occurred while approving the citizen" });
        }
    }


    /// <summary>
    /// Retrieves an application by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("reject/{id}")]
    public async Task<IActionResult> RejectCitizen(int id, string reason)
    {
        var result = await _citizenService.RejectCitizen(id, reason);
        return result == null ? NotFound() : Ok(result);
    }


    private int? GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value
            ?? User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return null;

        return userId;
    }
}

