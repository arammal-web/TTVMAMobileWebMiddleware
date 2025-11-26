using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a group of conditions used to control business process flows.
/// </summary>
[Table("ConditionGroups", Schema = "BP")]
public partial class ConditionGroup
{
    /// <summary>
    /// Unique identifier for the condition group
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Description of the condition group
    /// </summary>
    /// <example>Age Validation Conditions</example>
    [StringLength(50)]
    public string? Description { get; set; }

    /// <summary>
    /// ApplicationDomain classification (e.g., licensing, registration)
    /// </summary>
    /// <example>DriverLicensing</example>
    [StringLength(50)]
    [Unicode(false)]
    public string? Domain { get; set; }

    /// <summary>
    /// Indicates whether the record is logically deleted
    /// </summary>
    /// <example>false</example>
    public bool? IsDeleted { get; set; }

    /// <summary>
    /// Date the record was deleted (if applicable)
    /// </summary>
    /// <example>2025-03-01T12:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who deleted the record
    /// </summary>
    /// <example>7</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date when the record was created
    /// </summary>
    /// <example>2025-01-01T09:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record
    /// </summary>
    /// <example>3</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date of last modification
    /// </summary>
    /// <example>2025-02-15T10:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record
    /// </summary>
    /// <example>5</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Navigation property for all conditions under this condition group.
    /// </summary>
    [InverseProperty("ConditionGroup")]
    public virtual ICollection<Condition?> Conditions { get; set; } = new List<Condition>();
}
