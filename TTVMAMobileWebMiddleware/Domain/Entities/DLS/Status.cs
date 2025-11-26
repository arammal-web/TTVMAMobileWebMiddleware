using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

[Table("Status", Schema = "VRS")]
public partial class Status
{
    /// <summary>
    /// Unique identifier for the status
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// Short description of the status in English
    /// </summary>
    [StringLength(25)]
    public string StatusDesc { get; set; } = null!;

    /// <summary>
    /// Short description of the status in Arabic
    /// </summary>
    [StringLength(25)]
    public string? StatusDescAr { get; set; }

    /// <summary>
    /// Short description of the status in French
    /// </summary>
    [StringLength(25)]
    public string? StatusDescFr { get; set; }

    /// <summary>
    /// Navigation property for applications
    /// </summary>
    [InverseProperty("Status")]
    public virtual ICollection<ApplicationDLS?> Applications { get; set; } = new List<ApplicationDLS>();

    /// <summary>
    /// Navigation property for applications
    /// </summary>
    [InverseProperty("ApplicationApprovalStatus")]
    public virtual ICollection<ApplicationDLS?> ApplicationApprovalStatuses { get; set; } = new List<ApplicationDLS>();

    public ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();


    [InverseProperty("DrivingLicenseStatus")]
    public virtual ICollection<DrivingLicenseABP?> DrivingLicenses { get; set; } = new List<DrivingLicenseABP>();

}
