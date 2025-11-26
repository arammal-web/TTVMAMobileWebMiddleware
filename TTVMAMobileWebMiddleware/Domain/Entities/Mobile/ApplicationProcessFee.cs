using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[PrimaryKey("FeeId", "ProcessId", "BPVarietyId", "ApplicationId")]
[Table("ApplicationProcessFee")]
public partial class ApplicationProcessFee
{
    [Key]
    public int FeeId { get; set; }

    [Key]
    public int ProcessId { get; set; }

    [Key]
    public int BPVarietyId { get; set; }

    [Key]
    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    [StringLength(50)]
    public string FeeNameEn { get; set; } = null!;

    [StringLength(50)]
    public string FeeNameAr { get; set; } = null!;

    [StringLength(50)]
    public string FeeNameFr { get; set; } = null!;

    [Column(TypeName = "decimal(3, 2)")]
    public decimal? TaxPercentageApplicable { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? FeeTax { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? FeeValue { get; set; }

    public bool? IsPaid { get; set; }

    public int FeeCategoryId { get; set; }

    public int FeeTypeId { get; set; }

    [StringLength(50)]
    public string? FeeSP { get; set; }

    public bool IsMunicipalFee { get; set; }

    public bool CurrentYearIncluded { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal? DiscountAmount { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal? DiscountPercentage { get; set; }

    public bool IsExempted { get; set; }

    [StringLength(512)]
    public string? InvoiceNumber { get; set; }

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

    public int? StatusId { get; set; }

    [ForeignKey("ApplicationId, ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcessFees")]
    [JsonIgnore]
    public virtual ApplicationProcess ApplicationProcess { get; set; } = null!;

    [ForeignKey("FeeId")]
    [InverseProperty("ApplicationProcessFees")]
    [JsonIgnore]
    public virtual Fee Fee { get; set; } = null!;

    [ForeignKey("FeeCategoryId")]
    [InverseProperty("ApplicationProcessFees")]
    [JsonIgnore]
    public virtual FeeCategory FeeCategory { get; set; } = null!;

    [ForeignKey("FeeId, ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcessFees")]
    [JsonIgnore]
    public virtual ProcessFee ProcessFee { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime? PaidDate { get; set; }
}
