using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS
{
    [Table("Application", Schema = "APP")]
    public partial class ApplicationDLS
    {
        /// <summary>
        /// Unique identifier for the application
        /// </summary>
        /// <example>APP-2025-001</example>
        [Key]
        [StringLength(50)]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Application number
        /// </summary>
        /// <example>DL2025-45678</example>
        [StringLength(50)]
        public string ApplicationNumber { get; set; } = null!;

        /// <summary>
        /// Reference to the approval status of the application
        /// </summary>
        /// <example>2</example>
        public int ApplicationApprovalStatusId { get; set; }

        /// <summary>
        /// Date of the application Approval status
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? ApplicationApprovalStatusDate { get; set; }
        /// <summary>
        /// Type of application (e.g., REG, TRN)
        /// </summary>
        /// <example>REG</example>
        [StringLength(3)]
        [Unicode(false)]
        public string ApplicationTypeId { get; set; } = null!;
        /// <summary>
        /// Reference to the process for the application
        /// </summary>
        public int ProcessId { get; set; }
        /// <summary>
        /// Branch where the application was submitted
        /// </summary>
        /// <example>101</example>
        public int BranchId { get; set; }

        /// <summary>
        /// Department associated with the application
        /// </summary>
        /// <example>5</example>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Section within the department
        /// </summary>
        /// <example>3</example>
        public int? SectionId { get; set; }


        /// <summary>
        /// Unique identifier for the car
        /// </summary>
        /// <example>87654321</example>
        public int? CarUniqueNumber { get; set; }

        /// <summary>
        /// Identifier of the owner
        /// </summary>
        /// <example>2</example>
        public int OwnerId { get; set; }

        /// <summary>
        /// Full name of the owner
        /// </summary>
        /// <example>John Doe</example>
        public string? OwnerFullName { get; set; } = null!;

        /// <summary>
        /// Ownership type identifier
        /// </summary>
        /// <example>1</example>
        public int OwnershipTypeId { get; set; }

        /// <summary>
        /// Indicates whether the vehicle has ownership
        /// </summary>
        /// <example>true</example>
        public bool HasOwnership { get; set; }

        /// <summary>
        /// Identifier of the ownership record
        /// </summary>
        /// <example>789</example>
        public int? OwnershipId { get; set; }

        /// <summary>
        /// Indicates if the vehicle has an active license
        /// </summary>
        /// <example>true</example>
        public bool HasActiveLicense { get; set; }

        /// <summary>
        /// Identifier for the license
        /// </summary>
        /// <example>5012</example>
        public int? LicenseId { get; set; }

        /// <summary>
        /// Indicates if the vehicle is registered and active
        /// </summary>
        /// <example>true</example>
        public bool HasActiveRegisteredVehicle { get; set; }

        /// <summary>
        /// Identifier for the registered vehicle
        /// </summary>
        /// <example>1600</example>
        public int? RegisteredVehicleId { get; set; }

        /// <summary>
        /// Indicates if the application is exempted
        /// </summary>
        /// <example>false</example>
        public bool? IsExempted { get; set; }

        /// <summary>
        /// Indicates if the application was loaded manually
        /// </summary>
        /// <example>true</example>
        public bool? IsLoaded { get; set; }

        /// <summary>
        /// User ID who loaded the application
        /// </summary>
        /// <example>12</example>
        public int? LoadedByUserId { get; set; }

        /// <summary>
        /// List of invoice numbers linked to this application
        /// </summary>
        /// <example>INV-2345</example>
        [StringLength(50)]
        public string? InvoiceNumbers { get; set; }

        /// <summary>
        /// Identifier for the status of the application
        /// </summary>
        /// <example>4</example>
        public int? StatusId { get; set; }

        /// <summary>
        /// Date of the application status
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? ApplicationStatusDate { get; set; }

        /// <summary>
        /// Comments provided for the application
        /// </summary>
        /// <example>Urgent processing requested</example>
        [StringLength(500)]
        public string? Comments { get; set; }

        /// <summary>
        /// Flag for logical deletion
        /// </summary>
        /// <example>false</example>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Deletion timestamp
        /// </summary>
        /// <example>2025-06-01</example>
        [Column(TypeName = "smalldatetime")]
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// User ID who deleted the record
        /// </summary>
        /// <example>10</example>
        public int? DeletedUserId { get; set; }

        /// <summary>
        /// Date the record was created
        /// </summary>
        /// <example>2025-04-15</example>
        [Column(TypeName = "smalldatetime")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// User ID who created the record
        /// </summary>
        /// <example>1</example>
        public int? CreatedUserId { get; set; }

        /// <summary>
        /// Last modification date
        /// </summary>
        /// <example>2025-05-10</example>
        [Column(TypeName = "smalldatetime")]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// User ID who last modified the record
        /// </summary>
        /// <example>2</example>
        public int? ModifiedUserId { get; set; }

        /// <summary>
        /// List of related agendas
        /// </summary>
        [InverseProperty("Application")]
        public virtual ICollection<Agenda?> Agenda { get; set; } = new List<Agenda>();

        /// <summary>
        /// List of application processes
        /// </summary>
        [InverseProperty("Application")]
        public virtual ICollection<ApplicationProcess?> ApplicationProcesses { get; set; } = new List<ApplicationProcess>();

        /// <summary>
        /// List of application requested plates
        /// </summary>
        [InverseProperty("Application")]
        public virtual ICollection<ApplicationRequestedPlatesInfo?> ApplicationRequestedPlatesInfos { get; set; } = new List<ApplicationRequestedPlatesInfo>();

        /// <summary>
        /// Application type
        /// </summary>
        [ForeignKey("ApplicationTypeId")]
        [InverseProperty("Applications")]
        public virtual ApplicationDomain? ApplicationDomain { get; set; } = null!;

        /// <summary>
        /// Application approval status
        /// </summary>
        [ForeignKey("ApplicationApprovalStatusId")]
        [InverseProperty("ApplicationApprovalStatuses")]
        public virtual Status? ApplicationApprovalStatus { get; set; } = null!;

        /// <summary>
        /// Application status
        /// </summary>
        [ForeignKey("StatusId")]
        [InverseProperty("Applications")]
        public virtual Status? Status { get; set; }

        /// <summary>
        /// Application owner
        /// </summary>
        [ForeignKey("OwnerId")]
        [InverseProperty("Applications")]
        public virtual CitizenABP? Citizen { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();

        [InverseProperty("Application")]
        public virtual ICollection<DrivingLicenseABP> DrivingLicenses { get; set; } = new List<DrivingLicenseABP>();

        [InverseProperty("Application")]
        public virtual ICollection<OperationRequestABP?> OperationRequests { get; set; } = new List<OperationRequestABP>();


    }
}