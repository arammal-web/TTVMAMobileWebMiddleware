using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[PrimaryKey("ApplicationId", "ProcessId", "BPVarietyId", "ConditionId")]
[Table("ApplicationProcessCondition")]
public partial class ApplicationProcessCondition
{
    [Key]
    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    [Key]
    public int ProcessId { get; set; }

    [Key]
    public int BPVarietyId { get; set; }

    [Key]
    public int ConditionId { get; set; }

    public int ConditionGroupId { get; set; }

    public int? ConditionCode { get; set; }

    public int? CitizenAge { get; set; }

    public int? Age1 { get; set; }

    public int? Age2 { get; set; }

    public int? Value1 { get; set; }

    public int? Value2 { get; set; }

    public int? Value3 { get; set; }

    public int? LeadTime { get; set; }

    public bool IsValid { get; set; }

    public bool IsStopProcess { get; set; }

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

    [ForeignKey("ApplicationId, ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcessConditions")]
    [JsonIgnore]
    public virtual ApplicationProcess ApplicationProcess { get; set; } = null!;

    [ForeignKey("ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcessConditions")]
    [JsonIgnore]
    public virtual VarietyBusinessProcess VarietyBusinessProcess { get; set; } = null!;
}
