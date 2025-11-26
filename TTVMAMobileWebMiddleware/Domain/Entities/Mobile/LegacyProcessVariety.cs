using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[PrimaryKey("BPVarietyLegacyId", "BPVarietyId")]
[Table("LegacyProcessVariety")]
public partial class LegacyProcessVariety
{
    [Key]
    public int BPVarietyLegacyId { get; set; }

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
    [InverseProperty("LegacyProcessVarietyBPVarieties")]
    public virtual ProcessVariety BPVariety { get; set; } = null!;

    [ForeignKey("BPVarietyLegacyId")]
    [InverseProperty("LegacyProcessVarietyBPVarietyLegacies")]
    public virtual ProcessVariety BPVarietyLegacy { get; set; } = null!;
}
