using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

public partial class Fee
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Code { get; set; } = null!;

    public long? FeeCode { get; set; }

    [StringLength(100)]
    public string? FeeNameEn { get; set; }

    [StringLength(100)]
    public string? FeeNameAr { get; set; }

    [StringLength(100)]
    public string? FeeNameFr { get; set; }

    public int? FeeType { get; set; }

    public double? FeeValue { get; set; }

    public int FeeCategoryId { get; set; }

    public int StatusId { get; set; }

    [StringLength(50)]
    public string? Domain { get; set; }

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

    [InverseProperty("Fee")]
    public virtual ICollection<ApplicationProcessFee> ApplicationProcessFees { get; set; } = new List<ApplicationProcessFee>();

    [ForeignKey("FeeCategoryId")]
    [InverseProperty("Fees")]
    public virtual FeeCategory FeeCategory { get; set; } = null!;

    [InverseProperty("Fee")]
    public virtual ICollection<ProcessExemptionFee> ProcessExemptionFees { get; set; } = new List<ProcessExemptionFee>();

    [InverseProperty("Fee")]
    public virtual ICollection<ProcessFee> ProcessFees { get; set; } = new List<ProcessFee>();

    [ForeignKey("StatusId")]
    [InverseProperty("Fees")]
    public virtual Status Status { get; set; } = null!;
}
