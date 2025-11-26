using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a group of conditions used to control business process flows.
/// </summary>
[Table("CheckListGroups", Schema = "BP")]
public partial class CheckListGroup
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
    /// <example>Group 1</example>
    [StringLength(50)]
    public string? DescriptionEn { get; set; }

    /// <summary>
    /// Description of the condition group
    /// </summary>
    /// <example>Group 1</example>
    [StringLength(50)]
    public string? DescriptionAr { get; set; }

    /// <summary>
    /// Description of the condition group
    /// </summary>
    /// <example>Group 1</example>
    [StringLength(50)]
    public string? DescriptionFr { get; set; }

    /// <summary>
    /// Indicates whether the group is mandatory
    /// </summary>
    /// <example>true</example>
    public bool? IsMandatory { get; set; }

    /// <summary>
    /// Reference to the primary business process
    /// </summary>
    /// <example>1</example>
    public int? PrimaryProcessId { get; set; }

    /// <summary>
    /// Reference to the migration
    /// </summary>
    /// <example>1</example>
    public int? MigrationId { get; set; }

    /// <summary>
    /// Reference to the domain
    /// </summary>
    /// <example>1</example>
    [StringLength(50)]
    public string? Domain { get; set; }

    /// <summary>
    /// Indicates if the record is logically deleted
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date of record deletion
    /// </summary>
    /// <example>2025-01-20T16:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who deleted the record
    /// </summary>
    /// <example>1</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date of record creation
    /// </summary>
    /// <example>2025-01-20T16:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record
    /// </summary>
    /// <example>1</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date of last modification
    /// </summary>
    /// <example>2025-01-20T16:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record
    /// </summary>
    /// <example>1</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// The icon of the condition
    /// </summary>
    public string? Icon { get; set; }
    /// <summary>
    /// Collection of associated check lists
    /// </summary>
    [InverseProperty("CheckListGroup")]
    public virtual ICollection<CheckList?> CheckLists { get; set; } = new List<CheckList>();
}
