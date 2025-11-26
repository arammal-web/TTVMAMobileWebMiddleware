using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("ApplicationLog")]
public partial class ApplicationLog
{
    [Key]
    public int LogID { get; set; }

    [StringLength(10)]
    public string OperationType { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime OperationTimestamp { get; set; }

    [StringLength(50)]
    public string ExecutedByUserID { get; set; } = null!;

    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    [StringLength(50)]
    public string ApplicationNumber { get; set; } = null!;

    public int BranchId { get; set; }

    public int? DepartmentId { get; set; }

    public int? SectionId { get; set; }

    public int CarUniqueNumber { get; set; }

    public int OwnerId { get; set; }

    public int? OwnershipTypeId { get; set; }

    public bool HasOwnership { get; set; }

    public int? OwnershipId { get; set; }

    public bool HasActiveLicense { get; set; }

    public int? LicenseId { get; set; }

    public bool HasActiveRegisteredVehicle { get; set; }

    public int? RegisteredVehicleId { get; set; }

    public bool? IsExempted { get; set; }

    public bool? IsLoaded { get; set; }

    public int? LoadedByUserId { get; set; }

    [StringLength(50)]
    public string? InvoiceNumbers { get; set; }

    public int? StatusId { get; set; }

    [StringLength(500)]
    public string? Comments { get; set; }

    public bool? IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    public int? DeletedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int? CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }
}
