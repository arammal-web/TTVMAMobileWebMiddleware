using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("PasswordReset")]
[Index("Id", "ExpiresAtUtc", Name = "IX_PasswordReset_User_Expiry")]
[Index("TokenHash", Name = "UX_PasswordReset_TokenHash", IsUnique = true)]
public partial class PasswordReset
{
    /// <summary>
    /// Primary key for the password reset request record.
    /// </summary>
    [Key]
    [Column("Id")]
    public long ResetId { get; set; }

    /// <summary>
    /// Foreign key to AppUser.Id requesting the password reset.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Hash of the password reset token. Plaintext is never stored.
    /// </summary>
    [StringLength(256)]
    public string? TokenHash { get; set; } = null!;

    /// <summary>
    /// UTC datetime when the reset token expires.
    /// </summary>
    [Precision(3)]
    public DateTime? ExpiresAtUtc { get; set; }

    /// <summary>
    /// UTC datetime when the reset token was successfully used.
    /// </summary>
    [Precision(3)]
    public DateTime? ConsumedAtUtc { get; set; }

    /// <summary>
    /// UTC datetime when this password reset request was created.
    /// </summary>
    [Precision(3)]
    public DateTime? CreatedAtUtc { get; set; }

    [ForeignKey("Id")]
    [InverseProperty("PasswordResets")]
    public virtual AppUser? User { get; set; } = null!;
}
