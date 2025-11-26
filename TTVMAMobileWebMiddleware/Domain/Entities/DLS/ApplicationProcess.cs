using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents the link between an application and its associated business process and variety.
/// </summary>
[PrimaryKey("ApplicationId", "ProcessId", "BPVarietyId")]
[Table("ApplicationProcess", Schema = "APP")]
public partial class ApplicationProcess
{
    /// <summary>
    /// Reference to the application record
    /// </summary>
    /// <example>APP-2025-001</example>
    [Key]
    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    /// <summary>
    /// Reference to the business process related to the application
    /// </summary>
    /// <example>3</example>
    [Key]
    public int ProcessId { get; set; }

    /// <summary>
    /// Reference to the associated BusinessProcesses table
    /// </summary>
    /// <example>1</example>
    [Key]
    public int BPVarietyId { get; set; }

    /// <summary>
    /// Current status of the application process
    /// </summary>
    /// <example>5</example>
    public int? StatusId { get; set; }

    /// <summary>
    /// The related driving license identifier, if applicable
    /// </summary>
    /// <example>5</example>
    public int? DrivingLicenseId { get; set; }

    /// <summary>
    /// Total fee amount associated with the process
    /// </summary>
    /// <example>100000</example>
    [Column(TypeName = "decimal(18, 0)")]
    public decimal? TotalFee { get; set; }

    /// <summary>
    /// Total tax amount applicable to the process
    /// </summary>
    /// <example>11000</example>
    [Column(TypeName = "decimal(18, 0)")]
    public decimal? TotalTax { get; set; }

    /// <summary>
    /// Additional notes for the secondary process of the application
    /// </summary>
    /// <example>Urgent processing required</example>
    [StringLength(250)]
    public string? Notes { get; set; }

    /// <summary>
    /// Logical deletion flag for soft delete
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// ID of the user who deleted the record
    /// </summary>
    /// <example>101</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Timestamp when the record was marked as deleted
    /// </summary>
    /// <example>2025-06-01</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// Record creation date
    /// </summary>
    /// <example>2025-04-15</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record
    /// </summary>
    /// <example>1</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Last modification date of the record
    /// </summary>
    /// <example>2025-06-10</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record
    /// </summary>
    /// <example>3</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Navigation to the associated application
    /// </summary>
    [ForeignKey("ApplicationId")]
    [InverseProperty("ApplicationProcesses")]
    public virtual ApplicationDLS? Application { get; set; } = null!;

    /// <summary>
    /// Navigation to related checklist items
    /// </summary>
    [InverseProperty("ApplicationProcess")]
    public virtual ICollection<ApplicationProcessCheckList?> ApplicationProcessCheckLists { get; set; } = new List<ApplicationProcessCheckList>();

    /// <summary>
    /// Navigation to related process conditions
    /// </summary>
    [InverseProperty("ApplicationProcess")]
    public virtual ICollection<ApplicationProcessCondition?> ApplicationProcessConditions { get; set; } = new List<ApplicationProcessCondition>();

    /// <summary>
    /// Navigation to related documents
    /// </summary>
    [InverseProperty("ApplicationProcess")]
    public virtual ICollection<ApplicationProcessDocument?> ApplicationProcessDocuments { get; set; } = new List<ApplicationProcessDocument>();

    /// <summary>
    /// Navigation to associated process fees
    /// </summary>
    [InverseProperty("ApplicationProcess")]
    public virtual ICollection<ApplicationProcessFee?> ApplicationProcessFees { get; set; } = new List<ApplicationProcessFee>();

    /// <summary>
    /// Navigation to related business process variety
    /// </summary>
    [ForeignKey("ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcesses")]
    public virtual VarietyBusinessProcess? VarietyBusinessProcess { get; set; } = null!;

    /// <summary>
    /// Navigation to related driving license details
    /// </summary>
    [InverseProperty("ApplicationProcess")]
    public virtual ICollection<DrivingLicenseDetailABP> DrivingLicenseDetails { get; set; } = new List<DrivingLicenseDetailABP>();

}