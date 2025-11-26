using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("ProcessVariety")]
public partial class ProcessVariety
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    public string NameEn { get; set; } = null!;

    [StringLength(150)]
    public string NameAr { get; set; } = null!;

    [StringLength(50)]
    public string? Domain { get; set; }

    public int? ProcessVarietyTypeId { get; set; }

    public int? MigrationID { get; set; }

    public bool IsDeleted { get; set; }

    public int? DeletedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    [StringLength(20)]
    public string? Icon { get; set; }

    [StringLength(150)]
    public string? NameFr { get; set; }

    [InverseProperty("BPVariety")]
    public virtual ICollection<LegacyProcessVariety> LegacyProcessVarietyBPVarieties { get; set; } = new List<LegacyProcessVariety>();

    [InverseProperty("BPVarietyLegacy")]
    public virtual ICollection<LegacyProcessVariety> LegacyProcessVarietyBPVarietyLegacies { get; set; } = new List<LegacyProcessVariety>();

    [InverseProperty("BPVariety")]
    public virtual ICollection<PrerequisiteProcessVariety> PrerequisiteProcessVarietyBPVarieties { get; set; } = new List<PrerequisiteProcessVariety>();

    [InverseProperty("BPVarietyPrerequisite")]
    public virtual ICollection<PrerequisiteProcessVariety> PrerequisiteProcessVarietyBPVarietyPrerequisites { get; set; } = new List<PrerequisiteProcessVariety>();

    [InverseProperty("BPVariety")]
    public virtual ICollection<ProcessExemptionFee> ProcessExemptionFees { get; set; } = new List<ProcessExemptionFee>();

    [ForeignKey("ProcessVarietyTypeId")]
    [InverseProperty("ProcessVarieties")]
    public virtual ProcessVarietyType? ProcessVarietyType { get; set; }

    [InverseProperty("BPVariety")]
    public virtual ICollection<VarietyBusinessProcess> VarietyBusinessProcesses { get; set; } = new List<VarietyBusinessProcess>();
    [InverseProperty("BPVariety")]
    public virtual ICollection<DrivingLicenseDetail> DrivingLicenseDetails { get; set; } = new List<DrivingLicenseDetail>();

}
