using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TTVMAMobileWebMiddleware.Domain.Enums;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

/// <summary>
/// Represents the link between an online citizen and a local DLS citizen
/// </summary>
[Table("CitizenLink")]
public partial class CitizenLink
{
    /// <summary>
    /// Primary key - Online citizen ID
    /// </summary>
    [Key]
    public int CitizenOnlineId { get; set; }

    /// <summary>
    /// Local DLS citizen ID
    /// </summary>
    public int CitizenLocalId { get; set; }

    /// <summary>
    /// Method used to link the citizens
    /// </summary>
    public LinkMethod LinkMethod { get; set; }

    /// <summary>
    /// Confidence score of the match (0.0 to 1.0)
    /// </summary>
    [Column(TypeName = "decimal(3, 2)")]
    public decimal Confidence { get; set; }

    /// <summary>
    /// User ID who created the link
    /// </summary>
    public int LinkedByUserId { get; set; }

    /// <summary>
    /// UTC timestamp when the link was created
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime LinkedAtUtc { get; set; }

    /// <summary>
    /// Optional decision note from the officer
    /// </summary>
    [StringLength(1000)]
    public string? DecisionNote { get; set; }

    /// <summary>
    /// Navigation to online citizen
    /// </summary>
    [ForeignKey("CitizenOnlineId")]
    [InverseProperty("CitizenLinks")]
    public virtual Citizen? Citizen { get; set; }
}

