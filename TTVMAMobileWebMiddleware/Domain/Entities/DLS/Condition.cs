using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a condition that can be applied to a business process based on certain logic.
/// </summary>
[Table("Condition", Schema = "BP")]
public partial class Condition
{
    /// <summary>
    /// Unique identifier for the condition.
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Reference to the condition group.
    /// </summary>
    /// <example>10</example>
    public int ConditionGroupId { get; set; }

    /// <summary>
    /// Custom code used to identify the condition.
    /// </summary>
    /// <example>500</example>
    public int? ConditionCode { get; set; }

    /// <summary>
    /// Reference to the related business process.
    /// </summary>
    /// <example>2001</example>
    public int ProcessId { get; set; }

    /// <summary>
    /// Reference to the associated license type.
    /// </summary>
    /// <example>101</example>
    public int BPVarietyId { get; set; }

    /// <summary>
    /// Age range lower limit (if applicable).
    /// </summary>
    /// <example>18</example>
    public int? Age1 { get; set; }

    /// <summary>
    /// Age range upper limit (if applicable).
    /// </summary>
    /// <example>60</example>
    public int? Age2 { get; set; }

    /// <summary>
    /// Optional value for conditional logic.
    /// </summary>
    /// <example>100</example>
    public int? Value1 { get; set; }

    /// <summary>
    /// Optional value for conditional logic.
    /// </summary>
    /// <example>200</example>
    public int? Value2 { get; set; }

    /// <summary>
    /// Optional value for conditional logic.
    /// </summary>
    /// <example>300</example>
    public int? Value3 { get; set; }

    /// <summary>
    /// Lead time in days related to the condition.
    /// </summary>
    /// <example>30</example>
    public int? LeadTime { get; set; }

    /// <summary>
    /// Indicates whether the process should be stopped when the condition is met.
    /// </summary>
    /// <example>true</example>
    public bool? IsStopProcess { get; set; }

    /// <summary>
    /// Indicates whether the user should be notified when the condition is met.
    /// </summary>
    /// <example>true</example>
    public bool? IsUserNotification { get; set; }

    /// <summary>
    /// Notes or explanation for the condition.
    /// </summary>
    /// <example>Applicant must be over 18 years old.</example>
    [StringLength(500)]
    public string? Notes { get; set; } = null!;

    /// <summary>
    /// Indicates if the condition is logically deleted.
    /// </summary>
    /// <example>false</example>
    public bool? IsDeleted { get; set; }

    /// <summary>
    /// Date the record was deleted.
    /// </summary>
    /// <example>2025-02-10T13:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who deleted the record.
    /// </summary>
    /// <example>4</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date the record was created.
    /// </summary>
    /// <example>2025-01-05T10:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record.
    /// </summary>
    /// <example>2</example>
    public int? CreatedUserId { get; set; }

    /// <summary>
    /// Date the record was last modified.
    /// </summary>
    /// <example>2025-01-20T16:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record.
    /// </summary>
    /// <example>5</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Navigation to application process conditions that use this condition.
    /// </summary>
    [InverseProperty("Condition")]
    public virtual ICollection<ApplicationProcessCondition?> ApplicationProcessConditions { get; set; } = new List<ApplicationProcessCondition>();

    /// <summary>
    /// Navigation reference to the condition group.
    /// </summary>
    [ForeignKey("ConditionGroupId")]
    [InverseProperty("Conditions")]
    public virtual ConditionGroup? ConditionGroup { get; set; } = null!;

    /// <summary>
    /// Navigation reference to the related business process variety.
    /// </summary>
    [ForeignKey("ProcessId, BPVarietyId")]
    [InverseProperty("Conditions")]
    public virtual VarietyBusinessProcess? VarietyBusinessProcess { get; set; } = null!;
}