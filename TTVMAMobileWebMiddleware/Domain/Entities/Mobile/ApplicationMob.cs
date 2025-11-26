using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TTVMAMobileWebMiddleware.Domain.Entities.DLS;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("Application")]
public partial class ApplicationMob
{
    [Key]
    [StringLength(50)]
    public string Id { get; set; } = null!;

    [StringLength(50)]
    public string ApplicationNumber { get; set; } = null!;

    public int ApplicationApprovalStatusId { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string ApplicationTypeId { get; set; } = null!;

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
    public int?  ProcessId { get; set; }

    [StringLength(500)]
    public string? Comments { get; set; }

    public bool? IsDeleted { get; set; }
     
    public DateTime? DeletedDate { get; set; }

    public int? DeletedUserId { get; set; }
     
    public DateTime CreatedDate { get; set; }

    public int? CreatedUserId { get; set; } 
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    [StringLength(150)]
    public string? OwnerFullName { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime ApplicationDate { get; set; }
    
    [Column(TypeName = "smalldatetime")]
    public DateTime? ApplicationApprovalStatusDate { get; set; }
    
    [Column(TypeName = "smalldatetime")]
    public DateTime? ApplicationStatusDate { get; set; }

    // Navigation properties
    [InverseProperty("Application")]
    [JsonIgnore]
    public virtual ICollection<Agendum> Agenda { get; set; } = new List<Agendum>();

    [ForeignKey("ApplicationApprovalStatusId")]
    [InverseProperty("Applications")]
    [JsonIgnore]
    public virtual Status ApplicationApprovalStatus { get; set; } = null!;

    [InverseProperty("Application")]
    [JsonIgnore]
    public virtual ICollection<ApplicationProcess> ApplicationProcesses { get; set; } = new List<ApplicationProcess>();

    [InverseProperty("Application")]
    [JsonIgnore]
    public virtual ICollection<ApplicationRequestedPlatesInfo> ApplicationRequestedPlatesInfos { get; set; } = new List<ApplicationRequestedPlatesInfo>();
 
    [InverseProperty("Application")]
    [JsonIgnore]
    public virtual ICollection<DrivingTestRequest> DrivingTestRequests { get; set; } = new List<DrivingTestRequest>();

    [InverseProperty("Application")]
    [JsonIgnore]
    public virtual ICollection<OperationRequest> OperationRequests { get; set; } = new List<OperationRequest>();

    [ForeignKey("OwnerId")]
    [InverseProperty("Applications")]
    [JsonIgnore]
    public virtual Citizen? Citizen { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("ApplicationsWithStatus")]
    [JsonIgnore]
    public virtual Status? Status { get; set; }

    [ForeignKey("ApplicationTypeId")]
    [InverseProperty("Applications")]
    [JsonIgnore]
    public virtual ApplicationType? ApplicationType { get; set; }

    [InverseProperty("Application")]
    [JsonIgnore]
    public virtual ICollection<DrivingLicense> DrivingLicenses { get; set; } = new List<DrivingLicense>();


    public virtual ICollection<ReceiptMOB> Receipts { get; set; } = new List<ReceiptMOB>();
}
