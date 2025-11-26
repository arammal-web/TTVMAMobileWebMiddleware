using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("RefreshToken")]
[Index("Id", "ExpiresAtUtc", Name = "IX_RefreshToken_User_Expiry")]
[Index("TokenHash", Name = "UX_RefreshToken_TokenHash", IsUnique = true)]
public partial class RefreshToken
{
    /// <summary>
    /// Primary key for the refresh token record.
    /// </summary>
    [Key]
    [Column("Id")]
    public long Id { get; set; }

    /// <summary>
    /// Foreign key to AppUser.Id that owns this refresh token.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Hashed refresh token (never store plaintext).
    /// </summary>
    [StringLength(259)]
    public string? TokenHash { get; set; } = null!;

    /// <summary>
    /// UTC date/time when the refresh token was created.
    /// </summary>
    [Precision(3)]
    public DateTime? IssuedAtUtc { get; set; }

    /// <summary>
    /// UTC date/time when the refresh token will expire.
    /// </summary>
    [Precision(3)]
    public DateTime? ExpiresAtUtc { get; set; }

    /// <summary>
    /// UTC date/time when the refresh token was revoked.
    /// </summary>
    [Precision(3)]
    public DateTime? RevokedAtUtc { get; set; }

    /// <summary>
    /// Reason for refresh token revocation.
    /// </summary>
    [StringLength(64)]
    [Unicode(false)]
    public string? RevokedReason { get; set; }

    /// <summary>
    /// User-friendly device label from which this token was issued.
    /// </summary>
    [StringLength(128)]
    public string? DeviceName { get; set; }

    /// <summary>
    /// Unique fingerprint/identifier for the device.
    /// </summary>
    [StringLength(256)]
    public string? DeviceFingerprint { get; set; }

    /// <summary>
    /// IP address from which the token was issued.
    /// </summary>
    [StringLength(45)]
    [Unicode(false)]
    public string? IssuedIp { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("RefreshTokens")]
    public virtual AppUser? User { get; set; } = null!;
}
