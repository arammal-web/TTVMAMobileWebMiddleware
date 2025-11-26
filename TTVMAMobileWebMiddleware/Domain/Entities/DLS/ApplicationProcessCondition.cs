using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a condition linked to a specific application and business process.
/// </summary>
[PrimaryKey("ApplicationId", "ProcessId", "BPVarietyId", "ConditionId")]
[Table("ApplicationProcessCondition", Schema = "APP")]
public partial class ApplicationProcessCondition
{
    /// <summary>
    /// Application identifier this condition is linked to.
    /// </summary>
    /// <example>APP-2025-001</example>
    [Key]
    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    /// <summary>
    /// Business process identifier.
    /// </summary>
    /// <example>3</example>
    [Key]
    public int ProcessId { get; set; }

    /// <summary>
    /// License type identifier.
    /// </summary>
    /// <example>1</example>
    [Key]
    public int BPVarietyId { get; set; }

    /// <summary>
    /// Identifier for the associated condition.
    /// </summary>
    /// <example>12</example>
    [Key]
    public int ConditionId { get; set; }

    /// <summary>
    /// Identifier for the condition group.
    /// </summary>
    /// <example>5</example>
    public int ConditionGroupId { get; set; }

    /// <summary>
    /// Optional system-wide condition code.
    /// </summary>
    /// <example>108</example>
    public int? ConditionCode { get; set; }

    /// <summary>
    /// Age of the applicant.
    /// </summary>
    /// <example>22</example>
    public int? CitizenAge { get; set; }

    /// <summary>
    /// Age range start.
    /// </summary>
    /// <example>18</example>
    public int? Age1 { get; set; }

    /// <summary>
    /// Age range end.
    /// </summary>
    /// <example>65</example>
    public int? Age2 { get; set; }

    /// <summary>
    /// First validation value.
    /// </summary>
    /// <example>100</example>
    public int? Value1 { get; set; }

    /// <summary>
    /// Second validation value.
    /// </summary>
    /// <example>200</example>
    public int? Value2 { get; set; }

    /// <summary>
    /// Third validation value.
    /// </summary>
    /// <example>300</example>
    public int? Value3 { get; set; }

    /// <summary>
    /// Time in days before action must be taken.
    /// </summary>
    /// <example>10</example>
    public int? LeadTime { get; set; }

    /// <summary>
    /// Indicates if the condition is currently valid.
    /// </summary>
    /// <example>true</example>
    public bool IsValid { get; set; }

    /// <summary>
    /// Indicates whether this condition stops the process.
    /// </summary>
    /// <example>false</example>
    public bool IsStopProcess { get; set; }

    /// <summary>
    /// Indicates whether this condition triggers a user notification.
    /// </summary>
    /// <example>true</example>
    public bool IsUserNotification { get; set; }

    /// <summary>
    /// Additional notes for the condition.
    /// </summary>
    /// <example>Must be under 65 years old</example>
    [StringLength(500)]
    public string Notes { get; set; } = null!;

    /// <summary>
    /// Logical deletion flag.
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Deletion timestamp if the record is deleted.
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
    /// Record last modification timestamp.
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
    /// Navigation to the related application process.
    /// </summary>
    [ForeignKey("ApplicationId, ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcessConditions")]
    public virtual ApplicationProcess? ApplicationProcess { get; set; } = null!;

    /// <summary>
    /// Navigation to the related condition.
    /// </summary>
    [ForeignKey("ConditionId")]
    [InverseProperty("ApplicationProcessConditions")]
    public virtual Condition? Condition { get; set; } = null!;

    /// <summary>
    /// Navigation to the related business process variety.
    /// </summary>
    [ForeignKey("ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcessConditions")]
    public virtual VarietyBusinessProcess? VarietyBusinessProcess { get; set; } = null!;
}
