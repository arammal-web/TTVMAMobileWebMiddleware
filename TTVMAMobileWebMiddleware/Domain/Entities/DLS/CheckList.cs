using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a group of conditions used to control business process flows.
/// </summary>
[Table("CheckList", Schema = "BP")]
public partial class CheckList
{
    /// <summary>
    /// Primary key of the Conditions table
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Foreign key to the CheckListGroup table
    /// </summary>
    /// <example>1</example>
    public int CheckListGroupId { get; set; }

    /// <summary>
    /// Foreign key to the CheckListCode table
    /// </summary>
    /// <example>1</example>
    public int? CheckListCode { get; set; }

    /// <summary>
    /// The description of the condition
    /// </summary>
    /// <example>Condition 1</example>
    [StringLength(150)]
    public string? DescriptionEn { get; set; }

    /// <summary>
    /// The Arabic description of the condition
    /// </summary>
    /// <example>الشرط 1</example>
    [StringLength(150)]
    public string? DescriptionAr { get; set; }

    /// <summary>
    /// The French description of the condition
    /// </summary>
    /// <example>La condition 1</example>
    [StringLength(50)]
    public string? DescriptionFr { get; set; }

    /// <summary>
    /// Foreign key to the Migration table
    /// </summary>
    /// <example>1</example>
    public int? MigrationId { get; set; }

    /// <summary>
    /// The domain of the condition
    /// </summary>
    /// <example>DLS</example>
    [StringLength(50)]
    public string? Domain { get; set; }

    /// <summary>
    /// Indicates whether the record is deleted
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Timestamp when the record was deleted
    /// </summary>
    /// <example>2021-01-01 00:00:00.000</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who deleted the record
    /// </summary>
    /// <example>1</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Timestamp when the record was created
    /// </summary>
    /// <example>2021-01-01 00:00:00.000</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record
    /// </summary>
    /// <example>1</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Timestamp when the record was last modified
    /// </summary>
    /// <example>2021-01-01 00:00:00.000</example>
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
    /// Foreign key to the CheckListGroup table
    /// </summary>
    [ForeignKey("CheckListGroupId")]
    [InverseProperty("CheckLists")]
    public virtual CheckListGroup? CheckListGroup { get; set; } = null!;

    /// <summary>
    /// Foreign key to the ProcessCheckList table
    /// </summary>
    [InverseProperty("ChekList")]
    public virtual ICollection<ProcessCheckList?> ProcessCheckLists { get; set; } = new List<ProcessCheckList>();
}
