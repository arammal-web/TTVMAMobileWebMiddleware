using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a prerequisite relationship between process varieties.
/// </summary>
[PrimaryKey("BPVarietyPrerequisiteId", "BPVarietyId")]
[Table("PrerequisiteProcessVariety", Schema = "BP")]
public partial class PrerequisiteProcessVariety
{
    /// <summary>
    /// ID of the prerequisite process variety.
    /// </summary>
    /// <example>100</example>
    [Key]
    public int BPVarietyPrerequisiteId { get; set; }

    /// <summary>
    /// ID of the process variety that requires the prerequisite.
    /// </summary>
    /// <example>101</example>
    [Key]
    public int BPVarietyId { get; set; }

    /// <summary>
    /// Indicates whether the prerequisite record is deleted.
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date when the record was marked as deleted.
    /// </summary>
    /// <example>2025-01-31T12:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who deleted the record.
    /// </summary>
    /// <example>6</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date the record was created.
    /// </summary>
    /// <example>2025-01-10T08:00:00</example>
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
    /// <example>2025-01-20T14:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record.
    /// </summary>
    /// <example>4</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Reference to the current status of the prerequisite.
    /// </summary>
    /// <example>1</example>
    public int? StatusId { get; set; }

    /// <summary>
    /// Navigation property for the process variety.
    /// ForeignKey for BPVarietyId.
    /// InverseProperty for the Prerequisite of the Process Variety.
    /// </summary>
    [ForeignKey("BPVarietyId")]
    [InverseProperty("PrerequisiteProcessVarietyBPVarieties")]
    public virtual ProcessVariety? BPVariety { get; set; } = null!;

    /// <summary>
    /// Navigation property for the prerequisite process variety.
    /// ForeignKey for BPVarietyPrerequisiteId.
    /// InverseProperty for the Prerequisite of the Prerequisite Process Variety.
    /// </summary>
    [ForeignKey("BPVarietyPrerequisiteId")]
    [InverseProperty("PrerequisiteProcessVarietyBPVarietyPrerequisites")]
    public virtual ProcessVariety? BPVarietyPrerequisite { get; set; } = null!;
}
