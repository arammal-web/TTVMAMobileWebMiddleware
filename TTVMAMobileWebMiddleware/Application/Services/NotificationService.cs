using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Shared.RequestUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTVMAMobileWebMiddleware.Domain.Entities.Mobile;
using TTVMAMobileWebMiddleware.Application.Common;
using TTVMAMobileWebMiddleware.Application.Interfaces.Mobile;
using TTVMAMobileWebMiddleware.Domain.Entities;

namespace TTVMAMobileWebMiddleware.Application.Services
{
    /// <summary>
    /// Service implementation for managing notifications.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly MOBDbContext _context;
        private readonly IMemoryCache _cache;

        public NotificationService(MOBDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        /// <summary>
        /// Adds a new notification for a user.
        /// </summary>
        public async Task<Notification> AddAsync(Notification notification, CancellationToken ct = default)
        {
            try
            {
                if (notification == null)
                {
                    var ex = new Exception("Notification is required");
                    ex.HelpLink = "notification_required";
                    throw ex;
                }

                if (notification.UserId <= 0)
                {
                    var ex = new Exception("Notification UserId must be greater than 0");
                    ex.HelpLink = "notification_user_id_invalid";
                    throw ex;
                }

                if (string.IsNullOrWhiteSpace(notification.Title))
                {
                    var ex = new Exception("Notification Title is required");
                    ex.HelpLink = "notification_title_required";
                    throw ex;
                }

                if (string.IsNullOrWhiteSpace(notification.Message))
                {
                    var ex = new Exception("Notification Message is required");
                    ex.HelpLink = "notification_message_required";
                    throw ex;
                }

                notification.CreatedDate = notification.CreatedDate == default
                    ? DateTime.UtcNow
                    : notification.CreatedDate;

                if (notification.IsRead && notification.ReadDate == null)
                {
                    notification.ReadDate = DateTime.UtcNow;
                }

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync(ct);

                // Invalidate cache for the affected user
                _cache.Remove($"NotificationCount_User_{notification.UserId}");
                _cache.Remove($"UnreadNotificationCount_User_{notification.UserId}");
            }
            catch (Exception ex)
            {
                var exception = new Exception($"Error adding notification for user {notification?.UserId}");
                exception.HelpLink = "notification_add_error";
                throw exception;
            }
            return notification;
        }

        /// <summary>
        /// Retrieves all notifications for a specific user with pagination.
        /// </summary>
        public async Task<(IEnumerable<Notification> items, PaginationMetaData metaData)> GetByUserIdAsync(long userId, Pagination pagination, CancellationToken ct = default)
        {
            var query = _context.Notifications
                .Where(n => n.UserId == userId && (n.IsDeleted == null || n.IsDeleted == false))
                .OrderByDescending(n => n.CreatedDate);

            var cacheKey = $"NotificationCount_User_{userId}";

            var totalCount = await _cache.GetOrCreateAsync(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return query.CountAsync(ct);
            });

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync(ct);

            var metaData = PageList<Notification>.ToPageList(items, totalCount, pagination.PageNumber, pagination.PageSize).MetaData;
            return (items, metaData);
        }

        /// <summary>
        /// Retrieves unread notifications for a specific user with pagination.
        /// </summary>
        public async Task<(IEnumerable<Notification> items, PaginationMetaData metaData)> GetUnreadByUserIdAsync(long userId, Pagination pagination, CancellationToken ct = default)
        {
            var query = _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead && (n.IsDeleted == null || n.IsDeleted == false))
                .OrderByDescending(n => n.CreatedDate);

            var cacheKey = $"UnreadNotificationCount_User_{userId}";

            var totalCount = await _cache.GetOrCreateAsync(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return query.CountAsync(ct);
            });

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync(ct);

            var metaData = PageList<Notification>.ToPageList(items, totalCount, pagination.PageNumber, pagination.PageSize).MetaData;
            return (items, metaData);
        }

        /// <summary>
        /// Retrieves a notification by its ID.
        /// </summary>
        public async Task<Notification?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Notifications
                .Where(n => n.Id == id && (n.IsDeleted == null || n.IsDeleted == false))
                .FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        public async Task<bool> MarkAsReadAsync(int id, long userId, CancellationToken ct = default)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId && (n.IsDeleted == null || n.IsDeleted == false), ct);

            if (notification == null)
                return false;

            if (!notification.IsRead)
            {
                notification.IsRead = true;
                notification.ReadDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(ct);

                // Invalidate cache
                _cache.Remove($"NotificationCount_User_{userId}");
                _cache.Remove($"UnreadNotificationCount_User_{userId}");
            }

            return true;
        }

        /// <summary>
        /// Marks multiple notifications as read.
        /// </summary>
        public async Task<int> MarkMultipleAsReadAsync(IEnumerable<int> ids, long userId, CancellationToken ct = default)
        {
            var notificationIds = ids.ToList();
            if (!notificationIds.Any())
                return 0;

            var notifications = await _context.Notifications
                .Where(n => notificationIds.Contains(n.Id) && n.UserId == userId && (n.IsDeleted == null || n.IsDeleted == false) && !n.IsRead)
                .ToListAsync(ct);

            if (!notifications.Any())
                return 0;

            var readDate = DateTime.UtcNow;
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadDate = readDate;
            }

            await _context.SaveChangesAsync(ct);

            // Invalidate cache
            _cache.Remove($"NotificationCount_User_{userId}");
            _cache.Remove($"UnreadNotificationCount_User_{userId}");

            return notifications.Count;
        }

        /// <summary>
        /// Gets the count of unread notifications for a user.
        /// </summary>
        public async Task<int> GetUnreadCountAsync(long userId, CancellationToken ct = default)
        {
            var cacheKey = $"UnreadNotificationCount_User_{userId}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return await _context.Notifications
                    .CountAsync(n => n.UserId == userId && !n.IsRead && (n.IsDeleted == null || n.IsDeleted == false), ct);
            });
        }
    }
}

