using TTVMAMobileWebMiddleware.Application.Interfaces;
using TTVMAMobileWebMiddleware.Domain.Views;
using TTVMAMobileWebMiddleware.Domain.Entities;
using TTVMAMobileWebMiddleware.Domain.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Shared.RequestUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TTVMAMobileWebMiddleware.Application.Services
{
    /// <summary>
    /// Service implementation for managing village records.
    /// </summary>
    public class VillageService : IVillageService
    {
        private readonly MOBDbContext _context;
        private readonly IMemoryCache _cache;

        public VillageService(MOBDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        /// <summary>
        /// Retrieves all villages with pagination support.
        /// </summary>
        /// <returns>Tuple of village list and pagination metadata.</returns>
        /// <param name="pagination">Pagination filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <remarks>Returns paged data for UI grid or report views.</remarks>
        public async Task<(IEnumerable<VillageResponse> items, PaginationMetaData metaData)> 
            GetAllAsync(Pagination pagination,CancellationToken ct = default)
        {
            var query = _context.Villages
                .Where(v => v.IsActive);
            var cacheKey = "Village_Count";

            var totalCount = await _cache.GetOrCreateAsync(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return query.CountAsync(ct);
            });

            var items = await query
                .OrderBy(v => v.NameEN)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(v => new VillageResponse
                {
                    Id = v.Id,
                    NameEn = v.NameEN,
                    NameAr = v.NameAR,
                    NameFr = v.NameFR
                })
                .ToListAsync(ct);

            var pagedList = PageList<VillageResponse>.ToPageList(items, totalCount, pagination.PageNumber, pagination.PageSize);
            return (pagedList, pagedList.MetaData);
        }

        /// <summary>
        /// Retrieves a village entity by its primary ID.
        /// </summary>
        /// <returns>Village entity if found, else null.</returns>
        /// <param name="id">Village ID.</param>
        /// <remarks>Used for detail or update forms.</remarks>
        public async Task<VillageResponse?> GetByIdAsync(int id)
        {
            return await _context.Villages
                .Where(v => v.IsActive && v.Id == id)
                .Select(v => new VillageResponse
                {
                    Id = v.Id,
                    NameEn = v.NameEN,
                    NameAr = v.NameAR,
                    NameFr = v.NameFR
                })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves a key-value list of all active villages (ID, English name).
        /// </summary>
        /// <returns>List of key-value pairs for dropdowns.</returns>
        /// <remarks>Used in UI dropdowns for village selection.</remarks>
        public async Task<IEnumerable<KeyValuePair<int, string>>> GetVillageListAsync()
        {
            return await _context.Villages
                .Where(v => v.IsActive)
                .OrderBy(v => v.NameEN)
                .Select(v => new KeyValuePair<int, string>(v.Id, v.NameEN))
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves villages by country ID with pagination.
        /// </summary>
        /// <returns>Tuple of filtered list and pagination metadata.</returns>
        /// <param name="countryId">Country ID to filter by.</param>
        /// <param name="pagination">Pagination filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <remarks>Used for geographic filtering.</remarks>
        public async Task<(IEnumerable<VillageResponse> items, PaginationMetaData metaData)>
            GetByCountryIdAsync(int countryId, Pagination pagination, CancellationToken ct = default)
        {
            var query = _context.Villages
                .Where(v => v.IsActive && v.CountryId == countryId);
            var cacheKey = $"Village_Count_Country_{countryId}";

            var totalCount = await _cache.GetOrCreateAsync(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return query.CountAsync(ct);
            });

            var items = await query
                .OrderBy(v => v.NameEN)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(v => new VillageResponse
                {
                    Id = v.Id,
                    NameEn = v.NameEN,
                    NameAr = v.NameAR,
                    NameFr = v.NameFR
                })
                .ToListAsync(ct);

            var pagedList = PageList<VillageResponse>.ToPageList(items, totalCount, pagination.PageNumber, pagination.PageSize);
            return (pagedList, pagedList.MetaData);
        }

        /// <summary>
        /// Retrieves villages by prefecture ID with pagination.
        /// </summary>
        /// <returns>Tuple of filtered list and pagination metadata.</returns>
        /// <param name="prefectureId">Prefecture ID to filter by.</param>
        /// <param name="pagination">Pagination filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <remarks>Used when user selects a specific prefecture.</remarks>
        public async Task<(IEnumerable<VillageResponse> items, PaginationMetaData metaData)> 
            GetByPrefectureIdAsync(int prefectureId, Pagination pagination, CancellationToken ct = default)
        {
            var query = _context.Villages
                .Where(v => v.IsActive && v.PrefectureId == prefectureId);
            var cacheKey = $"Village_Count_Prefecture_{prefectureId}";

            var totalCount = await _cache.GetOrCreateAsync(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return query.CountAsync(ct);
            });

            var items = await query
                .OrderBy(v => v.NameEN)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(v => new VillageResponse
                {
                    Id = v.Id,
                    NameEn = v.NameEN,
                    NameAr = v.NameAR,
                    NameFr = v.NameFR
                })
                .ToListAsync(ct);

            var pagedList = PageList<VillageResponse>.ToPageList(items, totalCount, pagination.PageNumber, pagination.PageSize);
            return (pagedList, pagedList.MetaData);
        }

        /// <summary>
        /// Retrieves villages by region ID with pagination.
        /// </summary>
        /// <returns>Tuple of filtered list and pagination metadata.</returns>
        /// <param name="regionId">Region ID to filter by.</param>
        /// <param name="pagination">Pagination filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <remarks>Region can be null for unassigned villages.</remarks>
        public async Task<(IEnumerable<VillageResponse> items, PaginationMetaData metaData)> 
            GetByRegionIdAsync(int? regionId, Pagination pagination, CancellationToken ct = default)
        {
            var query = _context.Villages
                .Where(v => v.IsActive && v.RegionId == regionId);
            var cacheKey = $"Village_Count_Region_{regionId}";

            var totalCount = await _cache.GetOrCreateAsync(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return query.CountAsync(ct);
            });

            var items = await query
                .OrderBy(v => v.NameEN)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(v => new VillageResponse
                {
                    Id = v.Id,
                    NameEn = v.NameEN,
                    NameAr = v.NameAR,
                    NameFr = v.NameFR
                })
                .ToListAsync(ct);

            var pagedList = PageList<VillageResponse>.ToPageList(items, totalCount, pagination.PageNumber, pagination.PageSize);
            return (pagedList, pagedList.MetaData);
        }

        /// <summary>
        /// Retrieves villages by country, prefecture, and optional region ID with pagination.
        /// </summary>
        /// <returns>Tuple of filtered list and pagination metadata.</returns>
        /// <param name="countryId">Country ID.</param>
        /// <param name="prefectureId">Prefecture ID.</param>
        /// <param name="regionId">Optional Region ID.</param>
        /// <param name="pagination">Pagination filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <remarks>Used in cascading filters.</remarks>
        public async Task<(IEnumerable<VillageResponse> items, PaginationMetaData metaData)> 
            GetByLocationAsync(int countryId, int prefectureId, int? regionId, Pagination pagination, CancellationToken ct = default)
        {
            var query = _context.Villages
                .Where(v =>
                v.IsActive &&
                v.CountryId == countryId &&
                v.PrefectureId == prefectureId &&
                v.RegionId == regionId);

            var cacheKey = $"Village_Count_Location_{countryId}_{prefectureId}_{regionId}";

            var totalCount = await _cache.GetOrCreateAsync(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return query.CountAsync(ct);
            });

            var items = await query
                .OrderBy(v => v.NameEN)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(v => new VillageResponse
                {
                    Id = v.Id,
                    NameEn = v.NameEN,
                    NameAr = v.NameAR,
                    NameFr = v.NameFR
                })
                .ToListAsync(ct);

            var pagedList = PageList<VillageResponse>.ToPageList(items, totalCount, pagination.PageNumber, pagination.PageSize);
            return (pagedList, pagedList.MetaData);
        }
    }
}
