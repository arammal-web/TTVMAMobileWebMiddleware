using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("ProcessExemptionFee")]
public partial class ProcessExemptionFee
{
    [Key]
    public int Id { get; set; }

    public int FeeId { get; set; }

    public int BPId { get; set; }

    public int BPVarietyId { get; set; }

    [Column(TypeName = "decimal(3, 2)")]
    public decimal? TaxPercentageApplicable { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal ExemptionPercentage { get; set; }

    public int? StatusId { get; set; }

    public int? ExemptionTypeId { get; set; }

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

    [ForeignKey("BPId")]
    [InverseProperty("ProcessExemptionFees")]
    public virtual Process BP { get; set; } = null!;

    [ForeignKey("BPVarietyId")]
    [InverseProperty("ProcessExemptionFees")]
    public virtual ProcessVariety BPVariety { get; set; } = null!;

    [ForeignKey("ExemptionTypeId")]
    [InverseProperty("ProcessExemptionFees")]
    public virtual ExemptionType? ExemptionType { get; set; }

    [ForeignKey("FeeId")]
    [InverseProperty("ProcessExemptionFees")]
    public virtual Fee Fee { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("ProcessExemptionFees")]
    public virtual Status? Status { get; set; }
}
