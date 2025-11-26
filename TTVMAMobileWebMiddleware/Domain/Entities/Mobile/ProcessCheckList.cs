using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[PrimaryKey("ChekListId", "ProcessId", "BPVarietyId")]
[Table("ProcessCheckList")]
public partial class ProcessCheckList
{
    [Key]
    public int ChekListId { get; set; }

    [Key]
    public int ProcessId { get; set; }

    [Key]
    public int BPVarietyId { get; set; }

    public bool IsDocRequired { get; set; }

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

    [InverseProperty("ProcessCheckList")]
    public virtual ICollection<ApplicationProcessCheckList> ApplicationProcessCheckLists { get; set; } = new List<ApplicationProcessCheckList>();

    [ForeignKey("ChekListId")]
    [InverseProperty("ProcessCheckLists")]
    public virtual CheckList ChekList { get; set; } = null!;

    [ForeignKey("ProcessId, BPVarietyId")]
    [InverseProperty("ProcessCheckLists")]
    public virtual VarietyBusinessProcess VarietyBusinessProcess { get; set; } = null!;
}
