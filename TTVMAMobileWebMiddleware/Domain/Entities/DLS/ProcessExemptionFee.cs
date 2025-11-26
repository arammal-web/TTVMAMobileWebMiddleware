using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents an exemption fee applied to a specific process and license type.
/// </summary>
[Table("ProcessExemptionFee", Schema = "BP")]
public partial class ProcessExemptionFee
{
    /// <summary>
    /// Primary key of the ExemptionFee table.
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Reference to the Fees table.
    /// </summary>
    /// <example>15</example>
    public int FeeId { get; set; }

    /// <summary>
    /// Reference to the BusinessProcesses table.
    /// </summary>
    /// <example>101</example>
    public int BPId { get; set; }

    /// <summary>
    /// Reference to the license type.
    /// </summary>
    /// <example>205</example>
    public int BPVarietyId { get; set; }

    /// <summary>
    /// Tax percentage applicable for the exemption.
    /// </summary>
    /// <example>5.00</example>
    [Column(TypeName = "decimal(3, 2)")]
    public decimal? TaxPercentageApplicable { get; set; }

    /// <summary>
    /// Percentage of the exemption.
    /// </summary>
    /// <example>100</example>
    [Column(TypeName = "decimal(18, 0)")]
    public decimal ExemptionPercentage { get; set; }

    /// <summary>
    /// Status of the exemption fee.
    /// </summary>
    /// <example>1</example>
    public int? StatusId { get; set; }

    /// <summary>
    /// Reference to the ExemptionTypes table.
    /// </summary>
    /// <example>2</example>
    public int? ExemptionTypeId { get; set; }

    /// <summary>
    /// Indicates whether the record is deleted.
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date when the record was marked as deleted.
    /// </summary>
    /// <example>2025-01-20T10:15:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// User ID who marked the record as deleted.
    /// </summary>
    /// <example>6</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date when the record was created.
    /// </summary>
    /// <example>2025-01-01T08:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// User ID of the creator of the record.
    /// </summary>
    /// <example>2</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date when the record was last modified.
    /// </summary>
    /// <example>2025-01-10T16:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// User ID of the last modifier of the record.
    /// </summary>
    /// <example>4</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Navigation property for the Business Process.
    /// ForeignKey of BPId.
    /// Inverse Propertyfor Process Exemption Fees.
    /// </summary>
    [ForeignKey("BPId")]
    [InverseProperty("ProcessExemptionFees")]
    public virtual Process? BP { get; set; } = null!;

    /// <summary>
    /// Navigation property for the Business Process Variety.
    /// ForeignKey of BPVarietyId.
    /// Inverse Property for Process Exemption Fees.
    /// </summary>
    [ForeignKey("BPVarietyId")]
    [InverseProperty("ProcessExemptionFees")]
    public virtual ProcessVariety? BPVariety { get; set; } = null!;

    /// <summary>
    /// Navigation property for the Exemption Type.
    /// ForeignKey of ExemptionTypeId.
    /// Inverse Property for Process Exemption Fees.
    /// </summary>
    [ForeignKey("ExemptionTypeId")]
    [InverseProperty("ProcessExemptionFees")]
    public virtual ExemptionType? ExemptionType { get; set; }
    
    /// <summary>
    /// Navigation property for the Fee.
    /// ForeignKey of FeeId.
    /// Inverse Property for Process Exemption Fees.
    /// </summary>
    [ForeignKey("FeeId")]
    [InverseProperty("ProcessExemptionFees")]
    public virtual Fee? Fee { get; set; }
}