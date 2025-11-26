using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a checklist item linked to a specific application and business process.
/// </summary>
[PrimaryKey("ApplicationId", "ProcessId", "BPVarietyId", "ChekListId")]
[Table("ApplicationProcessCheckList", Schema = "APP")]
public partial class ApplicationProcessCheckList
{
    /// <summary>
    /// Reference to the application record.
    /// </summary>
    /// <example>APP-2025-001</example>
    [Key]
    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    /// <summary>
    /// Reference to the associated business process.
    /// </summary>
    /// <example>3</example>
    [Key]
    public int ProcessId { get; set; }

    /// <summary>
    /// Reference to the license type.
    /// </summary>
    /// <example>1</example>
    [Key]
    public int BPVarietyId { get; set; }

    /// <summary>
    /// Identifier for the checklist item.
    /// </summary>
    /// <example>15</example>
    [Key]
    public int ChekListId { get; set; }

    /// <summary>
    /// Indicates if a document is required for this checklist item.
    /// </summary>
    /// <example>true</example>
    public bool IsDocRequired { get; set; }

    /// <summary>
    /// Additional notes for the checklist.
    /// </summary>
    /// <example>Attach proof of residence</example>
    [StringLength(250)]
    public string? Notes { get; set; }

    /// <summary>
    /// Logical deletion flag.
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Deletion timestamp if record is deleted.
    /// </summary>
    /// <example>2025-05-01</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who deleted the record.
    /// </summary>
    /// <example>101</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Record creation timestamp.
    /// </summary>
    /// <example>2025-04-20</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record.
    /// </summary>
    /// <example>1</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Last modification timestamp.
    /// </summary>
    /// <example>2025-05-10</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record.
    /// </summary>
    /// <example>3</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Navigation property to the related application process.
    /// </summary>
    [ForeignKey("ApplicationId, ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcessCheckLists")]
    public virtual ApplicationProcess? ApplicationProcess { get; set; } = null!;

    /// <summary>
    /// Navigation property to related checklist document files.
    /// </summary>
    [InverseProperty("ApplicationProcessCheckList")]
    public virtual ICollection<ApplicationProcessCheckListDocFile?> ApplicationProcessCheckListDocFiles { get; set; } = new List<ApplicationProcessCheckListDocFile>();

    /// <summary>
    /// Navigation property to the process checklist.
    /// </summary>
    [ForeignKey("ChekListId, ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcessCheckLists")]
    public virtual ProcessCheckList? ProcessCheckList { get; set; } = null!;
}
