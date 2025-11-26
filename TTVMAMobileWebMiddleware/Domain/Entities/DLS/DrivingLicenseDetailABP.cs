using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

[Table("DrivingLicenseDetail", Schema = "DLS")]
public partial class DrivingLicenseDetailABP
{
    /// <summary>
    /// Unique identifier for each driving license detail record
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Reference to the driving license associated with this detail
    /// </summary>
    public int DrivingLicenseId { get; set; }

    /// <summary>
    /// Reference to the application record
    /// </summary>
    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    /// <summary>
    /// Reference to the business process related to the application
    /// </summary>
    public int ProcessId { get; set; }

    /// <summary>
    /// Reference to the associated BusinessProcesses table
    /// </summary>
    public int BPVarietyId { get; set; }

    /// <summary>
    /// The date when the driving license was issued
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime IssuingDate { get; set; }

    /// <summary>
    /// The expiry date of the driving license
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime ExpiryDate { get; set; }

    /// <summary>
    /// Optional description or additional details about the driving license
    /// </summary>
    [StringLength(250)]
    public string? Description { get; set; }

    /// <summary>
    /// Reference to the structure associated with this driving license detail
    /// </summary>
    public int? StructureId { get; set; }

    [StringLength(2000)]
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

    public int? ProcessVarietyTypeId { get; set; }

    [ForeignKey("ApplicationId, ProcessId, BPVarietyId")]
    [InverseProperty("DrivingLicenseDetails")]
    public virtual ApplicationProcess? ApplicationProcess { get; set; } = null!;

    [ForeignKey("BPVarietyId")]
    [InverseProperty("DrivingLicenseDetails")]
    public virtual ProcessVariety? BPVariety { get; set; } = null!;

    [ForeignKey("DrivingLicenseId")]
    [InverseProperty("DrivingLicenseDetails")]
    public virtual DrivingLicenseABP? DrivingLicense { get; set; } = null!;

    [ForeignKey("ProcessId")]
    [InverseProperty("DrivingLicenseDetails")]
    public virtual Process? Process { get; set; } = null!;
}
