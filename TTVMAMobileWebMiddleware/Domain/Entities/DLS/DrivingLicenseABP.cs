 using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

[Table("DrivingLicense", Schema = "DLS")]
public partial class DrivingLicenseABP
{
    /// <summary>
    /// Primary key of the DrivingLicense table
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Citizen ID associated with the driving license
    /// </summary>
    public int CitizenId { get; set; }

    /// <summary>
    /// Reference to the application record
    /// </summary>
    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    /// <summary>
    /// Unique driving license number
    /// </summary>
    [StringLength(50)]
    public string? DrivingLicenseNumber { get; set; }

    /// <summary>
    /// Status ID of the driving license
    /// </summary>
    public int? DrivingLicenseStatusId { get; set; }

    /// <summary>
    /// Date of the driving license status
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DrivingLicenseStatusDate { get; set; }

    /// <summary>
    /// Optional description of the driving license
    /// </summary>
    [StringLength(250)]
    public string? Description { get; set; }

    /// <summary>
    /// Date the driving license was issued
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime? IssuanceDate { get; set; }

    /// <summary>
    /// Date the driving license will expire
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// SAI code for the driving license
    /// </summary>
    [StringLength(50)]
    public string? SAI { get; set; }

    /// <summary>
    /// License codes associated with the driving license
    /// </summary>
    [StringLength(250)]
    public string? LicenseCodes { get; set; }

    /// <summary>
    /// Authority that issued the driving license
    /// </summary>
    public int? IssueAuthorityId { get; set; }

    public int? StructureId { get; set; }

    /// <summary>
    /// Number of points remaining on the driving license
    /// </summary>
    public int? NumberOfPoints { get; set; }

    /// <summary>
    /// Indicates if the driving license is currently blocked
    /// </summary>
    public bool? IsBlocked { get; set; }

    /// <summary>
    /// Indicates if the driving license is currently locked
    /// </summary>
    public bool? Islocked { get; set; }

    /// <summary>
    /// Authority responsible for blocking the license
    /// </summary>
    [StringLength(50)]
    public string? BlockingAuthority { get; set; }

    /// <summary>
    /// Reason why the license was blocked
    /// </summary>
    [StringLength(2000)]
    public string? BlockingReason { get; set; }

    /// <summary>
    /// Indicates if the license is an older version
    /// </summary>
    public bool? IsOldDrivingLicense { get; set; }

    /// <summary>
    /// Image of the old driving license
    /// </summary>
    public byte[]? OldDrivingLicenseImage { get; set; }

    public bool? IsByPasssTest { get; set; }

    [StringLength(150)]
    public string? ByPassTestAuthority { get; set; }

    [StringLength(250)]
    public string? ByPassTestReason { get; set; }

    public bool? IsPrinted { get; set; }

    [StringLength(150)]
    public string? PrintingMachine { get; set; }

    /// <summary>
    /// Updated after the card is printed
    /// </summary>
    [StringLength(50)]
    public string? CardSerialNumber { get; set; }

    /// <summary>
    /// JSON Data printed on the card
    /// </summary>
    public string? LicensePrintedData { get; set; }

    /// <summary>
    /// The hash of the printed data
    /// </summary>
    [StringLength(1000)]
    public string? DataHash { get; set; }

    public int? PrintedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? PrintedDate { get; set; }

    public int? PrintedStructureId { get; set; }

    [StringLength(2000)]
    public string? Notes { get; set; }

    public bool IsDeleted { get; set; }

    public int? DeletedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int? CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    [StringLength(150)]
    public string? CitizenFullName { get; set; }

    public int? DrivingLicenseTypeId { get; set; }

    public bool? IsInternational { get; set; }

    [ForeignKey("ApplicationId")]
    [InverseProperty("DrivingLicenses")]
    public virtual ApplicationDLS? Application { get; set; } = null!;

    [InverseProperty("DrivingLicense")]
    public virtual ICollection<DrivingLicenseDetailABP?> DrivingLicenseDetails { get; set; } = new List<DrivingLicenseDetailABP>();

    [ForeignKey("DrivingLicenseStatusId")]
    [InverseProperty("DrivingLicenses")]
    public virtual Status? DrivingLicenseStatus { get; set; } = null!;
    //[InverseProperty(nameof(Receipt.DrivingLicense))]
    //public virtual Receipt? Receipt { get; set; }
}
