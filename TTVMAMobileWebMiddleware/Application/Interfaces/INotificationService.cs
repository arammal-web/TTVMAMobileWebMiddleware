using Shared.RequestUtility;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

namespace TTVMAMobileWebMiddleware.Application.Interfaces.Mobile
{
    /// <summary>
    /// Service interface for managing notifications.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Retrieves all notifications for a specific user with pagination.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A tuple containing a list of notifications and pagination metadata.</returns>
        Task<(IEnumerable<Notification> items, PaginationMetaData metaData)> GetByUserIdAsync(long userId, Pagination pagination, CancellationToken ct = default);

        /// <summary>
        /// Retrieves unread notifications for a specific user with pagination.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A tuple containing a list of unread notifications and pagination metadata.</returns>
        Task<(IEnumerable<Notification> items, PaginationMetaData metaData)> GetUnreadByUserIdAsync(long userId, Pagination pagination, CancellationToken ct = default);

        /// <summary>
        /// Retrieves a notification by its ID.
        /// </summary>
        /// <param name="id">The ID of the notification.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The notification if found; otherwise, null.</returns>
        Task<Notification?> GetByIdAsync(int id, CancellationToken ct = default);

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        /// <param name="id">The ID of the notification to mark as read.</param>
        /// <param name="userId">The ID of the user marking the notification as read.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the notification was successfully marked as read; otherwise, false.</returns>
        Task<bool> MarkAsReadAsync(int id, long userId, CancellationToken ct = default);

        /// <summary>
        /// Marks multiple notifications as read.
        /// </summary>
        /// <param name="ids">The IDs of the notifications to mark as read.</param>
        /// <param name="userId">The ID of the user marking the notifications as read.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The number of notifications marked as read.</returns>
        Task<int> MarkMultipleAsReadAsync(IEnumerable<int> ids, long userId, CancellationToken ct = default);

        /// <summary>
        /// Gets the count of unread notifications for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The count of unread notifications.</returns>
        Task<int> GetUnreadCountAsync(long userId, CancellationToken ct = default);

        /// <summary>
        /// Adds a new notification for a user.
        /// </summary>
        /// <param name="notification">The notification to add.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The persisted notification.</returns>
        Task<Notification> AddAsync(Notification notification, CancellationToken ct = default);
    }
}

