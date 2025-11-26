using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a legacy relationship between two process varieties.
/// </summary>
[PrimaryKey("BPVarietyLegacyId", "BPVarietyId")]
[Table("LegacyProcessVariety", Schema = "BP")]
public partial class LegacyProcessVariety
{
    /// <summary>
    /// ID of the legacy process variety.
    /// </summary>
    /// <example>100</example>
    [Key]
    public int BPVarietyLegacyId { get; set; }

    /// <summary>
    /// ID of the current process variety.
    /// </summary>
    /// <example>101</example>
    [Key]
    public int BPVarietyId { get; set; }

    /// <summary>
    /// Indicates whether the record is deleted.
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date the record was marked as deleted.
    /// </summary>
    /// <example>2025-01-25T12:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who deleted the record.
    /// </summary>
    /// <example>8</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date the record was created.
    /// </summary>
    /// <example>2025-01-01T09:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record.
    /// </summary>
    /// <example>3</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date the record was last modified.
    /// </summary>
    /// <example>2025-01-15T15:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record.
    /// </summary>
    /// <example>5</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Reference to the current status of the legacy mapping.
    /// </summary>
    /// <example>1</example>
    public int? StatusId { get; set; }

    /// <summary>
    /// Navigation property for the current process variety.
    /// Inverse Property for LegacyProcess Variety by BPVarieties
    /// </summary>
    [ForeignKey("BPVarietyId")]
    [InverseProperty("LegacyProcessVarietyBPVarieties")]
    public virtual ProcessVariety? BPVariety { get; set; } = null!;

    /// <summary>
    /// Navigation property for the legacy process variety.
    /// Inverse Property for LegacyProcess Variety by BPVarieties Legacies
    /// </summary>
    [ForeignKey("BPVarietyLegacyId")]
    [InverseProperty("LegacyProcessVarietyBPVarietyLegacies")]
    public virtual ProcessVariety? BPVarietyLegacy { get; set; } = null!;
}
