using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents the detailed breakdown of fees associated with a specific application and business process.
/// </summary>
[PrimaryKey("FeeId", "ProcessId", "BPVarietyId", "ApplicationId")]
[Table("ApplicationProcessFee", Schema = "APP")]
public partial class ApplicationProcessFee
{
    /// <summary>
    /// The primary key of the ApplicationProcessFee table.
    /// </summary>
    /// <example>1</example>
    [Key]
    public int FeeId { get; set; }

    /// <summary>
    /// Foreign key referencing the BusinessProcesses table.
    /// </summary>
    /// <example>1</example>
    [Key]
    public int ProcessId { get; set; }

    /// <summary>
    /// Foreign key referencing the BusinessProcesses variety table.
    /// </summary>
    /// <example>1</example>
    [Key]
    public int BPVarietyId { get; set; }

    /// <summary>
    /// Foreign key referencing the Application table.
    /// </summary>
    /// <example>APP-2025-001</example>
    [Key]
    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    /// <summary>
    /// Detailed description of the fee in English.
    /// </summary>
    /// <example>Registration Fee</example>
    [StringLength(50)]
    public string FeeNameEn { get; set; } = null!;

    /// <summary>
    /// Detailed description of the fee in Arabic.
    /// </summary>
    /// <example>رسوم التسجيل</example>
    [StringLength(50)]
    public string FeeNameAr { get; set; } = null!;

    /// <summary>
    /// Detailed description of the fee in French.
    /// </summary>
    /// <example>Frais d'inscription</example>
    [StringLength(50)]
    public string FeeNameFr { get; set; } = null!;

    /// <summary>
    /// Percentage of tax applicable to the fee.
    /// </summary>
    /// <example>0.05</example>
    [Column(TypeName = "decimal(3, 2)")]
    public decimal? TaxPercentageApplicable { get; set; }

    /// <summary>
    /// Tax amount applied to the fee.
    /// </summary>
    /// <example>5000</example>
    [Column(TypeName = "decimal(18, 0)")]
    public decimal? FeeTax { get; set; }

    /// <summary>
    /// Monetary value of the fee.
    /// </summary>
    /// <example>150000</example>
    [Column(TypeName = "decimal(18, 0)")]
    public decimal? FeeValue { get; set; }

    /// <summary>
    /// Indicates if the fee has been paid.
    /// </summary>
    /// <example>true</example>
    public bool? IsPaid { get; set; }
    /// <summary>
    /// Date when the fee was paid.
    /// </summary>
    /// <example>2025-04-01T09:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? PaidDate { get; set; }

    /// <summary>
    /// Foreign key referencing the FeeCategory table.
    /// </summary>
    /// <example>2</example>
    public int FeeCategoryId { get; set; }

    /// <summary>
    /// Type of the fee applied.
    /// </summary>
    /// <example>1</example>
    public int FeeTypeId { get; set; }

    /// <summary>
    /// Service provider related to the fee.
    /// </summary>
    /// <example>LibanPost</example>
    [StringLength(50)]
    public string? FeeSP { get; set; }

    /// <summary>
    /// Indicates if the fee is a municipal fee.
    /// </summary>
    /// <example>false</example>
    public bool IsMunicipalFee { get; set; }

    /// <summary>
    /// Indicates if the fee is applicable to the current year.
    /// </summary>
    /// <example>true</example>
    public bool CurrentYearIncluded { get; set; }

    /// <summary>
    /// Amount of discount applied to the fee.
    /// </summary>
    /// <example>20000</example>
    [Column(TypeName = "decimal(19, 2)")]
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// Percentage of discount applied to the fee.
    /// </summary>
    /// <example>10</example>
    [Column(TypeName = "decimal(19, 2)")]
    public decimal? DiscountPercentage { get; set; }

    /// <summary>
    /// Indicates if the fee is exempted.
    /// </summary>
    /// <example>false</example>
    public bool IsExempted { get; set; }

    /// <summary>
    /// Invoice number associated with the fee.
    /// </summary>
    /// <example>INV-123456</example>
    [StringLength(512)]
    public string? InvoiceNumber { get; set; }

    /// <summary>
    /// Additional notes or remarks related to the fee.
    /// </summary>
    /// <example>Paid by proxy</example>
    [StringLength(500)]
    public string? Notes { get; set; }

    public bool IsDeleted { get; set; }
    /// <summary>
    /// Record deletion date.
    /// </summary>
    /// <example>2025-04-01T09:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who deleted the record.
    /// </summary>
    /// <example>10</example>
    public int DeletedUserId { get; set; }
    /// <summary>
    /// Record creation date.
    /// </summary>
    /// <example>2025-04-01T09:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record.
    /// </summary>
    /// <example>10</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Record modification date.
    /// </summary>
    /// <example>2025-04-01T09:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record.
    /// </summary>
    /// <example>10</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Status of the record.
    /// </summary>
    /// <example>1</example>
    public int? StatusId { get; set; }

    /// <summary>
    /// Navigation reference to ApplicationProcess.
    /// </summary>
    [ForeignKey("ApplicationId, ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcessFees")]
    public virtual ApplicationProcess? ApplicationProcess { get; set; } = null!;

    /// <summary>
    /// Navigation reference to Fee.
    /// </summary>
    [ForeignKey("FeeId")]
    [InverseProperty("ApplicationProcessFees")]
    public virtual Fee? Fee { get; set; } = null!;

    /// <summary>
    /// Navigation reference to ProcessFee.
    /// </summary>
    [ForeignKey("FeeId, ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcessFees")]
    public virtual ProcessFee? ProcessFee { get; set; } = null!;

    
}
