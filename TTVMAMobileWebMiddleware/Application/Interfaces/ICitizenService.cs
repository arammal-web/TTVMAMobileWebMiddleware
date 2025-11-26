using Shared.RequestUtility;
using System.Threading;
using System.Threading.Tasks;
using TTVMAMobileWebMiddleware.Application.DTOs;
using TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

namespace TTVMAMobileWebMiddleware.Application.Interfaces;

/// <summary>
/// Service interface for citizen validation operations
/// </summary>
public interface ICitizenService
{
    Task<CitizenLinkResponse> LinkAndApproveAsync(CitizenLinkRequest request, int userId, CancellationToken ct = default);
    Task<CitizenLinkResponse> CreateLocalAndApproveAsync(CitizenCreateLocalRequest request, int userId, CancellationToken ct = default);
    Task<CitizenLinkResponse> ReviewAndMergeAsync(CitizenLinkRequest request, int userId, CancellationToken ct = default);
    Task<bool> ApproveOnlineCitizenAsync(CitizenApproveRequest request, int userId, CancellationToken ct = default);
    Task<(IEnumerable<Citizen> items, PaginationMetaData metaData)> SearchMobileCitizen(Pagination pagination, CancellationToken ct = default);
    Task<CitizenSearchResponse> SearchLocalAsync(CitizenSearchRequest request, CancellationToken ct = default);
    Task<Citizen> GetOnlineCitizenById(int citizenId, CancellationToken ct = default);
    Task<bool> RejectCitizen(int id, string reason, CancellationToken ct = default);
}

