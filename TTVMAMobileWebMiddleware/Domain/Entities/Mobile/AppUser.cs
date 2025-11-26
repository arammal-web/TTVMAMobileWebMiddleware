using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("AppUser")]
public partial class AppUser
{
    /// <summary>
    /// Primary key for the application user account.
    /// </summary>
    [Key]
    [Column("Id")]
    public long Id { get; set; }

    [StringLength(256)]
    public string? Email { get; set; }

    /// <summary>
    /// Normalized email used for uniqueness checks (uppercased, trimmed).
    /// </summary>
    [StringLength(256)]
    public string? EmailNormalized { get; set; }

    public bool? IsEmailVerified { get; set; }

    [StringLength(32)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Normalized phone number used for uniqueness checks (E.164 format).
    /// </summary>
    [StringLength(32)]
    public string? PhoneNormalized { get; set; }

    public bool? IsPhoneVerified { get; set; }

    public byte[]? PasswordHash { get; set; }

    [StringLength(32)]
    [Unicode(false)]
    public string? PasswordAlgo { get; set; }

    public bool? TwoFactorEnabled { get; set; }

    [Precision(3)]
    public DateTime? LockoutUntilUtc { get; set; }

    public int? FailedAccessCount { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? PreferredLanguage { get; set; }

    public int? Status { get; set; }

    [Precision(3)]
    public DateTime? CreatedAtUtc { get; set; }

    [StringLength(45)]
    [Unicode(false)]
    public string? CreatedIp { get; set; }

    [Precision(3)]
    public DateTime? LastLoginAtUtc { get; set; }

    [Precision(3)]
    public DateTime? ModifiedAtUtc { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<LoginAudit>? LoginAudits { get; set; } = new List<LoginAudit>();

    [InverseProperty("User")]
    public virtual ICollection<OtpRequest>? OtpRequests { get; set; } = new List<OtpRequest>();

    [InverseProperty("User")]
    public virtual ICollection<PasswordReset>? PasswordResets { get; set; } = new List<PasswordReset>();

    [InverseProperty("User")]
    public virtual ICollection<RefreshToken>? RefreshTokens { get; set; } = new List<RefreshToken>();

    [InverseProperty("User")]
    public virtual Citizen? Citizen { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Notification>? Notifications { get; set; } = new List<Notification>();

}
