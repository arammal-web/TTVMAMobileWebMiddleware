using Shared.RequestUtility;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTVMAMobileWebMiddleware.Application.DTOs;
using TTVMAMobileWebMiddleware.Domain.Views;

namespace TTVMAMobileWebMiddleware.Application.Interfaces 
{
    /// <summary>
    /// Interface for managing Receipt entities.
    /// </summary>
    public interface IReceiptService
    {
       
        /// <summary>
        /// Creates a new receipt record with details.
        /// </summary>
        /// <param name="entity">Receipt with details entity to create</param>
        /// <returns>The created receipt</returns>
        Task<List<ReceiptResponseDto>> CreateWithDetailsAsync(ReceiptWithDetailRequest entity, CancellationToken ct = default);

        /// <summary>
        /// Syncs Receipt/ReceiptDetails and DrivingLicense/DrivingLicenseDetails from DLS database to Mobile database
        /// for all mobile app citizens and their applications.
        /// </summary>
        /// <param name="userId">User ID performing the sync operation</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Sync result with counts of synced records</returns>
        Task<SyncResult> SyncReceiptsAndDrivingLicensesAsync(int userId, CancellationToken ct = default);
    }
}
