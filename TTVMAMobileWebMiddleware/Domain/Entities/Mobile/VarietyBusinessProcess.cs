using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[PrimaryKey("ProcessId", "BPVarietyId")]
[Table("VarietyBusinessProcess")]
public partial class VarietyBusinessProcess
{
    [Key]
    public int ProcessId { get; set; }

    [Key]
    public int BPVarietyId { get; set; }

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

    public int? StatusId { get; set; }

    [InverseProperty("VarietyBusinessProcess")]
    public virtual ICollection<ApplicationProcessCondition> ApplicationProcessConditions { get; set; } = new List<ApplicationProcessCondition>();

    [InverseProperty("VarietyBusinessProcess")]
    public virtual ICollection<ApplicationProcess> ApplicationProcesses { get; set; } = new List<ApplicationProcess>();

    [ForeignKey("BPVarietyId")]
    [InverseProperty("VarietyBusinessProcesses")]
    public virtual ProcessVariety BPVariety { get; set; } = null!;

    [InverseProperty("VarietyBusinessProcess")]
    public virtual ICollection<DocumentProcessRelationship> DocumentProcessRelationships { get; set; } = new List<DocumentProcessRelationship>();

    [ForeignKey("ProcessId")]
    [InverseProperty("VarietyBusinessProcesses")]
    public virtual Process Process { get; set; } = null!;

    [InverseProperty("VarietyBusinessProcess")]
    public virtual ICollection<ProcessCheckList> ProcessCheckLists { get; set; } = new List<ProcessCheckList>();

    [InverseProperty("VarietyBusinessProcess")]
    public virtual ICollection<ProcessFee> ProcessFees { get; set; } = new List<ProcessFee>();

    [ForeignKey("StatusId")]
    [InverseProperty("VarietyBusinessProcesses")]
    public virtual Status? Status { get; set; }
}
