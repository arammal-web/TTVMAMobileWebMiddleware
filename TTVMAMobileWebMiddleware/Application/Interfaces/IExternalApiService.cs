using System.Threading;
using System.Threading.Tasks;
using TTVMAMobileWebMiddleware.Application.DTOs;
using TTVMAMobileWebMiddleware.Domain.Requests;
using TTVMAMobileWebMiddleware.Domain.Views;

namespace TTVMAMobileWebMiddleware.Application.Interfaces;

/// <summary>
/// Service interface for external API communication
/// </summary>
public interface IExternalApiService
{
    /// <summary>
    /// Sends the pending application DTO to external API endpoint
    /// </summary>
    /// <param name="dto">The pending application DTO to send</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Response from the external API</returns>
    Task<ExternalApiResponse> SendPendingApplicationAsync(ApprovePendingApplicationDTO dto, CancellationToken ct = default);

    /// <summary>
    /// Creates a new citizen in DLS with address and documents
    /// </summary>
    /// <param name="request">The citizen with details request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Response from the external API containing the created citizen ID</returns>
    Task<ExternalApiResponse> CreateCitizenWithAddressAndDocumentsAsync(CitizenWithDetailsRequest request, CancellationToken ct = default);

    /// <summary>
    /// Creates a new Receipt in DLS with Details
    /// </summary>
    /// <param name="request">The Receipt with details request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Response from the external API containing the created Receipt ID</returns>
    Task<ExternalApiResponse> CreateReceiptWithDetailsAsync(ReceiptWithDetailRequest request, CancellationToken ct = default);
}

