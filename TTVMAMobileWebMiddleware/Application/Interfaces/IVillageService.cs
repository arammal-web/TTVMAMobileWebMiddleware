using TTVMAMobileWebMiddleware.Domain.Views;
using TTVMAMobileWebMiddleware.Domain.Entities;
using TTVMAMobileWebMiddleware.Domain.Responses;
using Shared.RequestUtility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TTVMAMobileWebMiddleware.Application.Interfaces
{
    /// <summary>
    /// Service interface for handling village data operations including CRUD, filtering, and dropdown lookups.
    /// </summary>
    public interface IVillageService
    {
        // ================== CRUD OPERATIONS ==================

        /// <summary>
        /// Retrieves a paginated list of all villages.
        /// </summary>
        /// <returns>A tuple of paginated village views and pagination metadata.</returns>
        /// <param name="pagination">Pagination object including page number and size.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <remarks>Used for listing villages in grid views or reports.</remarks>
        Task<(IEnumerable<VillageResponse> items, PaginationMetaData metaData)>
            GetAllAsync(Pagination pagination,CancellationToken cancellationToken=default);

        /// <summary>
        /// Gets a village by its unique ID.
        /// </summary>
        /// <returns>The village entity if found; otherwise, null.</returns>
        /// <param name="id">Unique identifier of the village.</param>
        /// <remarks>Used in detail views or editing scenarios.</remarks>
        Task<VillageResponse?> GetByIdAsync(int id);

        // ================== FILTERED ACCESS ==================

        /// <summary>
        /// Gets villages by country ID with pagination support.
        /// </summary>
        /// <returns>Filtered list of villages with pagination metadata.</returns>
        /// <param name="countryId">The country ID to filter by.</param>
        /// <param name="pagination">Pagination information.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <remarks>Used to populate location data based on selected country.</remarks>
        Task<(IEnumerable<VillageResponse> items, PaginationMetaData metaData)>
            GetByCountryIdAsync(int countryId, Pagination pagination, CancellationToken cancellationToken =default);

        /// <summary>
        /// Gets villages by prefecture ID with pagination support.
        /// </summary>
        /// <returns>Filtered list of villages with pagination metadata.</returns>
        /// <param name="prefectureId">The prefecture ID to filter by.</param>
        /// <param name="pagination">Pagination information.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <remarks>Used to fetch villages under a specific administrative division.</remarks>
        Task<(IEnumerable<VillageResponse> items, PaginationMetaData metaData)> 
            GetByPrefectureIdAsync(int prefectureId, Pagination pagination, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets villages by optional region ID with pagination support.
        /// </summary>
        /// <returns>Filtered list of villages with pagination metadata.</returns>
        /// <param name="regionId">Optional region ID to filter villages.</param>
        /// <param name="pagination">Pagination information.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <remarks>Handles both regional and non-regional administrative hierarchies.</remarks>
        Task<(IEnumerable<VillageResponse> items, PaginationMetaData metaData)> 
            GetByRegionIdAsync(int? regionId, Pagination pagination, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets villages by country, prefecture, and optional region ID with pagination support.
        /// </summary>
        /// <returns>Filtered list of villages with pagination metadata.</returns>
        /// <param name="countryId">The country ID to filter by.</param>
        /// <param name="prefectureId">The prefecture ID to filter by.</param>
        /// <param name="regionId">Optional region ID to further filter villages.</param>
        /// <param name="pagination">Pagination information.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <remarks>Useful for geographic filtering in cascading dropdowns or location searches.</remarks>
        Task<(IEnumerable<VillageResponse> items, PaginationMetaData metaData)>
            GetByLocationAsync(int countryId, int prefectureId, int? regionId, Pagination pagination, CancellationToken cancellationToken = default);
        
        // ================== LOOKUPS ==================

        /// <summary>
        /// Gets a lightweight list of villages for dropdown menus or selection components.
        /// </summary>
        /// <returns>A list of village ID-name pairs.</returns>
        /// <remarks>Used in UI for fast binding of dropdowns.</remarks>
        Task<IEnumerable<KeyValuePair<int, string>>> GetVillageListAsync();
    }
}
