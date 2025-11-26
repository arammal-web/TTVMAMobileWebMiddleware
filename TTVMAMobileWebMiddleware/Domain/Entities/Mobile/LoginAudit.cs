using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("LoginAudit")]
[Index("Id", "EventAtUtc", Name = "IX_LoginAudit_User_EventDate", IsDescending = new[] { false, true })]
public partial class LoginAudit
{
    /// <summary>
    /// Primary key for the login audit record.
    /// </summary>
    [Key]
    [Column("Id")]
    public long Id { get; set; }

    /// <summary>
    /// Foreign key to AppUser.Id representing the user who attempted login.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Type of login event (e.g., login_success, login_failed, locked_out).
    /// </summary>
    [StringLength(32)]
    [Unicode(false)]
    public string? EventType { get; set; } = null!;

    /// <summary>
    /// UTC date/time when the event occurred.
    /// </summary>
    [Precision(3)]
    public DateTime? EventAtUtc { get; set; }

    /// <summary>
    /// Client IP address from which the login was attempted.
    /// </summary>
    [StringLength(45)]
    [Unicode(false)]
    public string? ClientIp { get; set; }

    /// <summary>
    /// Client agent string (browser, OS, app version).
    /// </summary>
    [StringLength(256)]
    public string? ClientAgent { get; set; }

    /// <summary>
    /// Optional details for the login attempt, such as error messages or location info.
    /// </summary>
    [StringLength(256)]
    public string? Detail { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("LoginAudits")]
    public virtual AppUser? User { get; set; } = null!;
}
