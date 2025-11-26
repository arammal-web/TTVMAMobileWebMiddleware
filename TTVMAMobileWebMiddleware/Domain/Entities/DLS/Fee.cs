using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a fee associated with a business process.
/// </summary>
[Table("Fees", Schema = "BP")]
public partial class Fee
{
    /// <summary>
    /// Unique identifier for the fee record
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Fee identifier code used within the system
    /// </summary>
    /// <example>DLF001</example>
    [StringLength(100)]
    public string Code { get; set; } = null!;

    /// <summary>
    /// External system fee code (if applicable)
    /// </summary>
    /// <example>45001</example>
    public long? FeeCode { get; set; }

    /// <summary>
    /// Name of the fee in English
    /// </summary>
    /// <example>Driving License Fee</example>
    [StringLength(100)]
    public string? FeeNameEn { get; set; }

    /// <summary>
    /// Name of the fee in Arabic
    /// </summary>
    /// <example>رسم رخصة القيادة</example>
    [StringLength(100)]
    public string? FeeNameAr { get; set; }

    /// <summary>
    /// Name of the fee in French
    /// </summary>
    /// <example>Frais de permis de conduire</example>
    [StringLength(100)]
    public string? FeeNameFr { get; set; }

    /// <summary>
    /// Type of the fee (linked to FeeType table)
    /// </summary>
    /// <example>2</example>
    public int? FeeType { get; set; }

    /// <summary>
    /// Value of the fee
    /// </summary>
    /// <example>150.00</example>
    public double? FeeValue { get; set; }

    /// <summary>
    /// Reference to the FeeCategory table
    /// </summary>
    /// <example>DL-CAT-001</example> 
    public int FeeCategoryId { get; set; }  

    /// <summary>
    /// Reference to the Status table
    /// </summary>
    /// <example>1</example>
    public int StatusId { get; set; }

    /// <summary>
    /// ApplicationDomain name for system partitioning
    /// </summary>
    /// <example>DriverLicensing</example>
    [StringLength(50)]
    public string? Domain { get; set; }

    /// <summary>
    /// Migration identifier
    /// </summary>
    /// <example>1234</example>
    public int? MigrationID { get; set; }

    /// <summary>
    /// Logical deletion flag (1 = deleted, 0 = active)
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// ID of the user who deleted the record
    /// </summary>
    /// <example>8</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date the record was deleted
    /// </summary>
    /// <example>2025-01-30T14:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// Date the record was created
    /// </summary>
    /// <example>2025-01-01T10:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// User who created the record
    /// </summary>
    /// <example>3</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date the record was last modified
    /// </summary>
    /// <example>2025-01-15T12:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// User who last modified the record
    /// </summary>
    /// <example>4</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Navigation property for the application process fees associated with this fee.
    /// </summary>
    [InverseProperty("Fee")]
    public virtual ICollection<ApplicationProcessFee?> ApplicationProcessFees { get; set; } = new List<ApplicationProcessFee>();

    /// <summary>
    /// Foreign key relationship to the fee category that this fee belongs to.
    /// </summary>
    [ForeignKey("FeeCategoryId")]
    [InverseProperty("Fees")]
    public virtual FeeCategory? FeeCategory { get; set; } = null!;

    /// <summary>
    /// Navigation property for the process fees associated with this fee.
    /// </summary>
    [InverseProperty("Fee")]
    public virtual ICollection<ProcessFee?> ProcessFees { get; set; } = new List<ProcessFee>();

   
    /// <summary>
    /// Navigation property for the receipts that include this fee.
    /// </summary>
    [InverseProperty("Fee")]
    public virtual ICollection<ProcessExemptionFee?> ProcessExemptionFees { get; set; } = new List<ProcessExemptionFee>();
}
