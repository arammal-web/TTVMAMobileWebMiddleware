using Microsoft.AspNetCore.Mvc;
using Shared.RequestUtility;
using System.Threading;
using System.Threading.Tasks;
using TTVMAMobileWebMiddleware.Application.DTOs;
using TTVMAMobileWebMiddleware.Domain.Entities.Mobile;
using TTVMAMobileWebMiddleware.Domain.Views;

namespace TTVMAMobileWebMiddleware.Application.Interfaces;

/// <summary>
/// Service interface for application approval operations
/// </summary>
public interface IApplicationService
{
    Task<(IEnumerable<ApplicationMob> items, PaginationMetaData metaData)> GetAllAsync(Pagination pagination, CancellationToken ct = default);
    Task<ApplicationMob?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<ApplicationDto?> GetByIdWithDetailAsync(string id, CancellationToken ct = default);
    Task<IActionResult> ApprovePendingApplication(string applicationId, CancellationToken ct = default);
    Task<bool> RejectApplication(string id, CancellationToken ct = default);
    Task<bool> DocumentRequiredApplication(string id, CancellationToken ct = default);

    Task<(IEnumerable<ApplicationListItemDto> items, PaginationMetaData metaData)> GetApplicationsAsync(Pagination pagination, string? keyword = null, int? status = null, string? filtration = "all", int? userId = null, int? branchId = null, CancellationToken ct = default);

}

