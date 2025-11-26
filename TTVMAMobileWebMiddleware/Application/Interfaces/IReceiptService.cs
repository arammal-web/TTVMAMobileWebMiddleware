using Shared.RequestUtility;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        Task<List<ReceiptResponseDto>> CreateWithDeatailsAsync(ReceiptWithDetailRequest entity);
         
    }
}
