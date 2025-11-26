using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a checklist item that must be completed or submitted for a specific process and variety.
/// </summary>
[PrimaryKey("ChekListId", "ProcessId", "BPVarietyId")]
[Table("ProcessCheckList", Schema = "BP")]
public partial class ProcessCheckList
{
    /// <summary>
    /// Unique identifier of the checklist item.
    /// </summary>
    /// <example>1</example>
    [Key]
    public int ChekListId { get; set; }

    /// <summary>
    /// Identifier of the associated business process.
    /// </summary>
    /// <example>101</example>
    [Key]
    public int ProcessId { get; set; }

    /// <summary>
    /// Reference to the associated BusinessProcesses table.
    /// </summary>
    /// <example>205</example>
    [Key]
    public int BPVarietyId { get; set; }

    /// <summary>
    /// Indicates whether a document is required for the checklist item.
    /// </summary>
    /// <example>true</example>
    public bool IsDocRequired { get; set; }

    /// <summary>
    /// Indicates whether the checklist item has been deleted.
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date when the checklist item was marked as deleted.
    /// </summary>
    /// <example>2025-01-22T10:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who deleted the checklist item.
    /// </summary>
    /// <example>7</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date when the checklist item was created.
    /// </summary>
    /// <example>2025-01-10T08:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the checklist item.
    /// </summary>
    /// <example>2</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date when the checklist item was last modified.
    /// </summary>
    /// <example>2025-01-20T16:45:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the checklist item.
    /// </summary>
    /// <example>5</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Inverse property for the Process CheckList.
    /// </summary>
    [InverseProperty("ProcessCheckList")]
    public virtual ICollection<ApplicationProcessCheckList?> ApplicationProcessCheckLists { get; set; } = new List<ApplicationProcessCheckList>();

    /// <summary>
    /// Navigation property for the CheckList.
    /// ForeignKey("ChekListId").
    /// Inverse Propertyfor Process CheckLists.
    /// </summary>
    [ForeignKey("ChekListId")]
    [InverseProperty("ProcessCheckLists")]
    public virtual CheckList? ChekList { get; set; } = null!;

    /// <summary>
    /// Navigation property for the Variety Business Process.
    /// ForeignKey("ProcessId, BPVarietyId")
    /// Inverse Property for Process Check Lists.
    /// </summary>
    [ForeignKey("ProcessId, BPVarietyId")]
    [InverseProperty("ProcessCheckLists")]
    public virtual VarietyBusinessProcess? VarietyBusinessProcess { get; set; } = null!;
}
