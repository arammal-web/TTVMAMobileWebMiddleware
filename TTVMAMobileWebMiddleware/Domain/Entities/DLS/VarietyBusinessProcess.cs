using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents the relationship between a business process and its associated process variety.
/// </summary>
[PrimaryKey("ProcessId", "BPVarietyId")]
[Table("VarietyBusinessProcess", Schema = "BP")]
public partial class VarietyBusinessProcess
{
    /// <summary>
    /// Reference to the main BusinessProcesses table.
    /// </summary>
    /// <example>101</example>
    [Key]
    public int ProcessId { get; set; }

    /// <summary>
    /// Reference to the associated BusinessProcesses variety table.
    /// </summary>
    /// <example>205</example>
    [Key]
    public int BPVarietyId { get; set; }

    /// <summary>
    /// Indicates whether the record is marked as deleted.
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date when the record was marked as deleted.
    /// </summary>
    /// <example>2024-12-31T00:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who deleted the record.
    /// </summary>
    /// <example>10</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date when the record was created.
    /// </summary>
    /// <example>2024-01-01T09:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record.
    /// </summary>
    /// <example>1</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date when the record was last modified.
    /// </summary>
    /// <example>2024-03-15T15:45:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record.
    /// </summary>
    /// <example>3</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Reference to the current status of the record.
    /// </summary>
    /// <example>2</example>
    public int? StatusId { get; set; }

    [InverseProperty("VarietyBusinessProcess")]
    public virtual ICollection<ApplicationProcessCondition?> ApplicationProcessConditions { get; set; } = new List<ApplicationProcessCondition>();

    [InverseProperty("VarietyBusinessProcess")]
    public virtual ICollection<ApplicationProcess?> ApplicationProcesses { get; set; } = new List<ApplicationProcess>();

    [ForeignKey("BPVarietyId")]
    [InverseProperty("VarietyBusinessProcesses")]
    public virtual ProcessVariety? BPVariety { get; set; } = null!;

    [InverseProperty("VarietyBusinessProcess")]
    public virtual ICollection<Condition?> Conditions { get; set; } = new List<Condition>();

    [InverseProperty("VarietyBusinessProcess")]
    public virtual ICollection<DocumentProcessRelationship?> DocumentProcessRelationships { get; set; } = new List<DocumentProcessRelationship>();

    [ForeignKey("ProcessId")]
    [InverseProperty("VarietyBusinessProcesses")]
    public virtual Process? Process { get; set; } = null!;

    [InverseProperty("VarietyBusinessProcess")]
    public virtual ICollection<ProcessCheckList?> ProcessCheckLists { get; set; } = new List<ProcessCheckList>();

    [InverseProperty("VarietyBusinessProcess")]
    public virtual ICollection<ProcessFee?> ProcessFees { get; set; } = new List<ProcessFee>();
}
