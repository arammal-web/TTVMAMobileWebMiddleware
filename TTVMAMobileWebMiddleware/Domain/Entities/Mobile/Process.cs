using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("Process")]
public partial class Process
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string? NameEn { get; set; }

    [StringLength(100)]
    public string? NameAr { get; set; }

    public int TypeId { get; set; }

    public int StatusId { get; set; }

    [StringLength(150)]
    public string? LandingPage { get; set; }

    public int? MigrationId { get; set; }

    [StringLength(50)]
    public string? Domain { get; set; }

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

    public bool? EnableVariety { get; set; }

    [StringLength(20)]
    public string? Icon { get; set; }

    [StringLength(100)]
    public string? NameFr { get; set; }
    public bool? IsDLRequired { get; set; }

    [InverseProperty("Process")]
    public virtual ICollection<ExemptionTypeDocRelationship> ExemptionTypeDocRelationships { get; set; } = new List<ExemptionTypeDocRelationship>();

    [InverseProperty("BP")]
    public virtual ICollection<ProcessExemptionFee> ProcessExemptionFees { get; set; } = new List<ProcessExemptionFee>();

    [ForeignKey("StatusId")]
    [InverseProperty("Processes")]
    public virtual Status Status { get; set; } = null!;

    [ForeignKey("TypeId")]
    [InverseProperty("Processes")]
    public virtual ProcessType Type { get; set; } = null!;

    [InverseProperty("Process")]
    public virtual ICollection<VarietyBusinessProcess> VarietyBusinessProcesses { get; set; } = new List<VarietyBusinessProcess>();

    [InverseProperty("Process")]
    public virtual ICollection<DrivingLicenseDetail> DrivingLicenseDetails { get; set; } = new List<DrivingLicenseDetail>();
}
 
