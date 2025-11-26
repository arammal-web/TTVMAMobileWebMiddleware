using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents the fee structure associated with a specific process and process variety.
/// </summary>
[PrimaryKey("FeeId", "ProcessId", "BPVarietyId")]
[Table("ProcessFee", Schema = "BP")]
public partial class ProcessFee
{
    /// <summary>
    /// Unique identifier for the process fee record.
    /// </summary>
    /// <example>1001</example>
    public int Id { get; set; }

    /// <summary>
    /// Reference to the Fees table.
    /// </summary>
    /// <example>15</example>
    [Key]
    public int FeeId { get; set; }

    /// <summary>
    /// Reference to the BusinessProcesses table.
    /// </summary>
    /// <example>101</example>
    [Key]
    public int ProcessId { get; set; }

    /// <summary>
    /// Reference to the LicenseTypes table.
    /// </summary>
    /// <example>205</example>
    [Key]
    public int BPVarietyId { get; set; }

    /// <summary>
    /// Applicable tax percentage for the fee.
    /// </summary>
    /// <example>5.00</example>
    [Column(TypeName = "decimal(3, 2)")]
    public decimal? TaxPercentageApplicable { get; set; }

    /// <summary>
    /// Base fee value applicable for the process.
    /// </summary>
    /// <example>150</example>
    [Column(TypeName = "decimal(18, 0)")]
    public decimal? FeeValue { get; set; }

    /// <summary>
    /// ApplicationDomain or context to which the fee applies.
    /// </summary>
    /// <example>DriverLicensing</example>
    [StringLength(50)]
    public string? Domain { get; set; }

    /// <summary>
    /// Identifier used during data migration.
    /// </summary>
    /// <example>2003</example>
    public int? MigrationID { get; set; }

    /// <summary>
    /// Logical deletion flag (true = deleted, false = active).
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date when the record was created.
    /// </summary>
    /// <example>2024-11-01T08:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record.
    /// </summary>
    /// <example>3</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date when the record was last modified.
    /// </summary>
    /// <example>2025-01-15T14:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record.
    /// </summary>
    /// <example>6</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Reference to the current status of the fee entry.
    /// </summary>
    /// <example>1</example>
    public int? StatusId { get; set; }
    
    /// <summary>
    /// Indicates whether the fee is mandatory for the process.
    /// </summary>
    public bool IsMandatory { get; set; } = true;


    /// <summary>
    /// Inverse Property for Process Fees.
    /// </summary>
    [InverseProperty("ProcessFee")]
    public virtual ICollection<ApplicationProcessFee?> ApplicationProcessFees { get; set; } = new List<ApplicationProcessFee>();
   
    /// <summary>
    /// Navigation property for the Fee.
    /// ForeignKey of FeeId.
    /// Inverse Property for Process Fees.
    /// </summary>
    [ForeignKey("FeeId")]
    [InverseProperty("ProcessFees")]
    public virtual Fee? Fee { get; set; } = null!;

    /// <summary>
    /// Navigation property for the Business Process.
    /// ForeignKey of ProcessId and BPVarietyId.
    /// Inverse Property for Process Fees.
    /// </summary>
    [ForeignKey("ProcessId, BPVarietyId")]
    [InverseProperty("ProcessFees")]
    public virtual VarietyBusinessProcess? VarietyBusinessProcess { get; set; } = null!;
}