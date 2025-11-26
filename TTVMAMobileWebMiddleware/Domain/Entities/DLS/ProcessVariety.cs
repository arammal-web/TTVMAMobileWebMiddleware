using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a category or variety of a business process within the system.
/// </summary>
[Table("ProcessVariety", Schema = "BP")]
public partial class ProcessVariety
{
    /// <summary>
    /// Unique identifier for the business process variety.
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Business process category name in English.
    /// </summary>
    /// <example>Standard License</example>
    [StringLength(150)]
    public string NameEn { get; set; } = null!;

    /// <summary>
    /// Business process category name in Arabic.
    /// </summary>
    /// <example>رخصة عادية</example>
    [StringLength(150)]
    public string NameAr { get; set; } = null!;

    /// <summary>
    /// Business process category name in French.
    /// </summary>
    [StringLength(150)]
    public string NameFr { get; set; } = null!;
    /// <summary>
    /// ApplicationDomain or context the process variety belongs to.
    /// </summary>
    /// <example>Transport</example>
    [StringLength(50)]
    public string? Domain { get; set; }

    /// <summary>
    /// the Weight of each Variety.
    /// </summary>
    /// <example>Transport</example>
    [StringLength(50)]
    public string? Weight { get; set; }

    /// <summary>
    /// Foreign key to the type of process variety.
    /// </summary>
    /// <example>3</example>
    public int? ProcessVarietyTypeId { get; set; }

    /// <summary>
    /// Identifier used for data migration.
    /// </summary>
    /// <example>102</example>
    public int? MigrationID { get; set; }

    /// <summary>
    /// Logical deletion flag (true = deleted, false = active).
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// ID of the user who deleted the record.
    /// </summary>
    /// <example>5</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date the record was marked as deleted.
    /// </summary>
    /// <example>NULL</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// Date the record was created.
    /// </summary>
    /// <example>2024-12-01T09:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record.
    /// </summary>
    /// <example>0</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date the record was last modified.
    /// </summary>
    /// <example>2025-02-10T15:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record.
    /// </summary>
    /// <example>3</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// The icon of the condition
    /// </summary>
    public string? Icon { get; set; }
    /// <summary>
    /// Inverse Property for Business Process Varieties.
    /// </summary>
    [InverseProperty("BPVariety")]
    public virtual ICollection<LegacyProcessVariety?> LegacyProcessVarietyBPVarieties { get; set; } = new List<LegacyProcessVariety>();

    /// <summary>
    /// Inverse Property for Business Process Varieties Legacy.
    /// </summary>
    [InverseProperty("BPVarietyLegacy")]
    public virtual ICollection<LegacyProcessVariety?> LegacyProcessVarietyBPVarietyLegacies { get; set; } = new List<LegacyProcessVariety>();

    /// <summary>
    /// Inverse Property for Business Process Varieties.
    /// </summary>
    [InverseProperty("BPVariety")]
    public virtual ICollection<PrerequisiteProcessVariety?> PrerequisiteProcessVarietyBPVarieties { get; set; } = new List<PrerequisiteProcessVariety>();

    /// <summary>
    /// Inverse Property for Business Process Varieties Prerequisite.
    /// </summary>
    [InverseProperty("BPVarietyPrerequisite")]
    public virtual ICollection<PrerequisiteProcessVariety?> PrerequisiteProcessVarietyBPVarietyPrerequisites { get; set; } = new List<PrerequisiteProcessVariety>();

    /// <summary>
    /// Inverse Property for Business Process Varieties.
    /// </summary>
    [InverseProperty("BPVariety")]
    public virtual ICollection<ProcessExemptionFee?> ProcessExemptionFees { get; set; } = new List<ProcessExemptionFee>();

    /// <summary>
    /// Navigation property for the Process Variety Type.
    /// ForeignKey of ProcessVarietyTypeId.
    /// Inverse Property for Process Varieties.
    /// </summary>
    [ForeignKey("ProcessVarietyTypeId")]
    [InverseProperty("ProcessVarieties")]
    public virtual ProcessVarietyType? ProcessVarietyType { get; set; }

    [InverseProperty("BPVariety")]
    public virtual ICollection<DrivingLicenseDetailABP> DrivingLicenseDetails { get; set; } = new List<DrivingLicenseDetailABP>();

    /// <summary>
    /// Inverse Property for Business Process Varieties.
    /// </summary>
    [InverseProperty("BPVariety")]
    public virtual ICollection<VarietyBusinessProcess?> VarietyBusinessProcesses { get; set; } = new List<VarietyBusinessProcess>();
}