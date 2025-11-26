using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[PrimaryKey("BPVarietyPrerequisiteId", "BPVarietyId")]
[Table("PrerequisiteProcessVariety")]
public partial class PrerequisiteProcessVariety
{
    [Key]
    public int BPVarietyPrerequisiteId { get; set; }

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

    [ForeignKey("BPVarietyId")]
    [InverseProperty("PrerequisiteProcessVarietyBPVarieties")]
    public virtual ProcessVariety BPVariety { get; set; } = null!;

    [ForeignKey("BPVarietyPrerequisiteId")]
    [InverseProperty("PrerequisiteProcessVarietyBPVarietyPrerequisites")]
    public virtual ProcessVariety BPVarietyPrerequisite { get; set; } = null!;
}
