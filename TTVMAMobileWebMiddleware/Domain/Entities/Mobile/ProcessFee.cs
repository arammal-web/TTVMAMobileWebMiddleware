using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[PrimaryKey("FeeId", "ProcessId", "BPVarietyId")]
[Table("ProcessFee")]
public partial class ProcessFee
{
    public int Id { get; set; }

    [Key]
    public int FeeId { get; set; }

    [Key]
    public int ProcessId { get; set; }

    [Key]
    public int BPVarietyId { get; set; }

    [Column(TypeName = "decimal(3, 2)")]
    public decimal? TaxPercentageApplicable { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? FeeValue { get; set; }

    [StringLength(50)]
    public string? Domain { get; set; }

    public int? MigrationID { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    public int? StatusId { get; set; }

    public bool? IsMandatory { get; set; }

    [InverseProperty("ProcessFee")]
    public virtual ICollection<ApplicationProcessFee> ApplicationProcessFees { get; set; } = new List<ApplicationProcessFee>();

    [ForeignKey("FeeId")]
    [InverseProperty("ProcessFees")]
    public virtual Fee Fee { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("ProcessFees")]
    public virtual Status? Status { get; set; }

    [ForeignKey("ProcessId, BPVarietyId")]
    [InverseProperty("ProcessFees")]
    public virtual VarietyBusinessProcess VarietyBusinessProcess { get; set; } = null!;
}
