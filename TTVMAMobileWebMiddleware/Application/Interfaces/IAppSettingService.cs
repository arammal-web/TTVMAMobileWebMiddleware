using TTVMAMobileWebMiddleware.Domain.Entities;
using TTVMAMobileWebMiddleware.Domain.Entities.DLS;

namespace TTVMAMobileWebMiddleware.Application.Interfaces 
{
    public interface IAppSettingService
    { 
        // Core lookups
        Task<AppSetting?> GetByCodeAsync(string paramCode, CancellationToken ct = default);
        Task<string?> GetValueAsync(string paramCode, CancellationToken ct = default);         // first non-empty value
        Task<string[]?> GetValuesAsync(string paramCode, CancellationToken ct = default);      // all non-empty values
        Task<T?> GetValueAsync<T>(string paramCode, CancellationToken ct = default);           // typed: first non-empty value

        // Existing one
        Task<string> GetAIUrlAsync(CancellationToken ct = default);

        // Convenience getters (typed)
        Task<int?> GetNumberOfTryAsync(CancellationToken ct = default);
        Task<int?> GetApplicationValidityWindowAsync(CancellationToken ct = default);
        Task<bool?> GetExamListAssignNowDefaultAsync(CancellationToken ct = default);
        Task<int?> GetExamListDefaultMaxSeatsAsync(CancellationToken ct = default);
    }
}
