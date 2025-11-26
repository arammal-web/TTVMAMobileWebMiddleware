using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("Condition")]
public partial class Condition
{
    [Key]
    public int Id { get; set; }

    public int ConditionGroupId { get; set; }

    public int? ConditionCode { get; set; }

    public int ProcessId { get; set; }

    public int BPVarietyId { get; set; }

    public int? Age1 { get; set; }

    public int? Age2 { get; set; }

    public int? Value1 { get; set; }

    public int? Value2 { get; set; }

    public int? Value3 { get; set; }

    public int? LeadTime { get; set; }

    public bool IsStopProcess { get; set; } = false;

    public bool IsUserNotification { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    public int? DeletedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    [ForeignKey("ConditionGroupId")]
    [InverseProperty("Conditions")]
    public virtual ConditionGroup ConditionGroup { get; set; } = null!;
}
