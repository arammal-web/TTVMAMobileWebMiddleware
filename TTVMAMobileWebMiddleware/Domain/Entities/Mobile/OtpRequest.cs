using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("OtpRequest")]
[Index("Target", "Purpose", "ConsumedAtUtc", "ExpiresAtUtc", Name = "IX_OtpRequest_TargetPurposeActive")]
public partial class OtpRequest
{
    /// <summary>
    /// Primary key for the OTP request record.
    /// </summary>
    [Key]
    [Column("Id")]
    public long Id { get; set; }

    /// <summary>
    /// Foreign key to AppUser.Id. NULL for OTPs issued before account creation.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Email or phone channel used to deliver the OTP.
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string? Channel { get; set; } = null!;

    /// <summary>
    /// Target recipient (email or phone) for the OTP.
    /// </summary>
    [StringLength(256)]
    public string? Target { get; set; } = null!;

    /// <summary>
    /// Purpose of the OTP request (e.g., register, login).
    /// </summary>
    [StringLength(32)]
    [Unicode(false)]
    public string? Purpose { get; set; } = null!;

    /// <summary>
    /// Hash of the one-time password code. Plain code is never stored.
    /// </summary>
    [StringLength(256)]
    public string? CodeHash { get; set; } = null!;

    /// <summary>
    /// Server-side expiry date/time for the OTP.
    /// </summary>
    [Precision(3)]
    public DateTime? ExpiresAtUtc { get; set; }

    /// <summary>
    /// Maximum number of allowed verification attempts.
    /// </summary>
    public int MaxAttempts { get; set; }

    /// <summary>
    /// Number of verification attempts already made.
    /// </summary>
    public int AttemptCount { get; set; }

    /// <summary>
    /// Date/time the OTP was successfully used (null if unused).
    /// </summary>
    [Precision(3)]
    public DateTime? ConsumedAtUtc { get; set; }

    /// <summary>
    /// IP address of the request originator.
    /// </summary>
    [StringLength(45)]
    [Unicode(false)]
    public string? RequestIp { get; set; }

    /// <summary>
    /// Timestamp when the OTP record was created.
    /// </summary>
    [Precision(3)]
    public DateTime CreatedAtUtc { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("OtpRequests")]
    public virtual AppUser? User { get; set; }
}
