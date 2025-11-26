using TTVMAMobileWebMiddleware.Domain.Views;
 

namespace TTVMAMobileWebMiddleware.Application.Validators
{
    /// <summary>
    /// Interface for the Driving License Validation Engine.
    /// Dynamically applies rules defined in the database (Condition and ConditionGroup).
    /// </summary>
    public interface IDLValidationEngineService
    {
        /// <summary>
        /// Validates the business process based on rules such as MinAge, Exam, Biometric, etc.
        /// </summary>
        /// <param name="processId">The ID of the driving license process (business process).</param>
        /// <param name="categoryIds">The IDs of the categories to validate.</param>
        /// <param name="applicantId">The ID of the applicant (optional for nationality-based checks).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A list of validation results including errors, warnings, and validity.</returns>
        Task<ProcessProfile> BusinessProcessValidationAsync(
        int processId, List<int> categoryIds, int applicantId, int ApplicationApprovalStatus = 0 , CancellationToken ct = default);


        /// <summary>
        /// Generates the menu for the application management page.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ActionMenu> GetApplicationActionMenuAsync(string applicationId, CancellationToken ct = default);
    }
}
