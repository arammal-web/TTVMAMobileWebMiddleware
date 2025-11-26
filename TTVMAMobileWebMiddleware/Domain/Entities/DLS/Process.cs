using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a business process that can be associated with one or more process types, statuses, and variations.
/// </summary>
[Table("Process", Schema = "BP")]
public partial class Process
{
    /// <summary>
    /// Unique identifier for the business process.
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Business process name in English.
    /// </summary>
    /// <example>Driving License Issuance</example>
    [StringLength(100)]
    public string? NameEn { get; set; }

    /// <summary>
    /// Business process name in Arabic.
    /// </summary>
    /// <example>إصدار رخصة قيادة</example>
    [StringLength(100)]
    public string? NameAr { get; set; }

    /// <summary>
    /// Business process name in French.
    /// </summary>
    [StringLength(100)]
    public string? NameFr { get; set; } 

    /// <summary>
    /// Reference to the type of the business process.
    /// </summary>
    /// <example>2</example>
    public int TypeId { get; set; }

    /// <summary>
    /// Reference to the current status of the business process.
    /// </summary>
    /// <example>1</example>
    public int StatusId { get; set; }

    /// <summary>
    /// URL of the landing page for the business process.
    /// </summary>
    /// <example>CreateCitizen url</example>
    public string? LandingPage { get; set; }

    /// <summary>
    /// Identifier used during data migration.
    /// </summary>
    /// <example>1005</example>
    public int? MigrationId { get; set; }

    /// <summary>
    /// The domain or context the process belongs to.
    /// </summary>
    /// <example>Licensing</example>
    [StringLength(50)]
    public string? Domain { get; set; }

    /// <summary>
    /// Indicates whether the process is marked as deleted.
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// ID of the user who deleted the process.
    /// </summary>
    /// <example>12</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date the process was marked as deleted.
    /// </summary>
    /// <example>2024-12-31T00:00:00</example> 
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// Date the process record was created.
    /// </summary>
    /// <example>2024-01-01T08:30:00</example> 
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the process record.
    /// </summary>
    /// <example>5</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date of the last modification to the process.
    /// </summary>
    /// <example>2024-02-01T14:45:00</example> 
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Indicates whether the process supports multiple process varieties.
    /// </summary>
    public string? EnableVariety { get; set; } 

    /// <summary>
    /// Icon associated with the business process.
    /// </summary>
    public string? Icon { get; set; }


    /// <summary>
    /// ID of the user who last modified the process record.
    /// </summary>
    /// <example>8</example>
    public int? ModifiedUserId { get; set; }
    /// <summary>
    /// Indicates whether a driving license is required for the business process.
    /// </summary>
    public bool? IsDLRequired { get; set; }
    /// <summary>
    /// URL of the secondary landing page for the business process.
    /// </summary>
    public string? SecondaryLandingPage { get; set; }

    [InverseProperty("Process")]
    public virtual ICollection<DrivingLicenseDetailABP> DrivingLicenseDetails { get; set; } = new List<DrivingLicenseDetailABP>();

    [InverseProperty("Process")]
    public virtual ICollection<ExemptionTypeDocRelationship?> ExemptionTypeDocRelationships { get; set; } = new List<ExemptionTypeDocRelationship>();

    [InverseProperty("BP")]
    public virtual ICollection<ProcessExemptionFee?> ProcessExemptionFees { get; set; } = new List<ProcessExemptionFee>();

    [ForeignKey("TypeId")]
    [InverseProperty("Processes")]
    public virtual ProcessType? Type { get; set; } = null!;

    [InverseProperty("Process")]
    public virtual ICollection<VarietyBusinessProcess?> VarietyBusinessProcesses { get; set; } = new List<VarietyBusinessProcess>();
}
