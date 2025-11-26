using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("DrivingTestRequest")]
public partial class DrivingTestRequest
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    public int ProcessId { get; set; }

    public int BPVarietyId { get; set; }

    public int OperationTypeId { get; set; }

    public int RequestStatusId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime RequestStatusDate { get; set; }

    [StringLength(50)]
    public string? Description { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? TestDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? TestTime { get; set; }

    public int? TestExamResultId { get; set; }

    public int StructureId { get; set; }

    [StringLength(2000)]
    public string? Notes { get; set; }

    public bool? IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    public int? DeletedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? CreatedDate { get; set; }

    public int? CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    [StringLength(50)]
    public string ApplicationNumber { get; set; } = null!;

    [StringLength(50)]
    public string ReceiptNumber { get; set; } = null!;

    public int CitizenId { get; set; }

    [ForeignKey("ApplicationId")]
    [InverseProperty("DrivingTestRequests")]
    public virtual ApplicationMob Application { get; set; } = null!;

    [ForeignKey("ApplicationId, ProcessId, BPVarietyId")]
    [InverseProperty("DrivingTestRequests")]
    public virtual ApplicationProcess ApplicationProcess { get; set; } = null!;

    [ForeignKey("OperationTypeId")]
    [InverseProperty("DrivingTestRequests")]
    public virtual OperationType OperationType { get; set; } = null!;

    [ForeignKey("RequestStatusId")]
    [InverseProperty("DrivingTestRequests")]
    public virtual RequestStatus RequestStatus { get; set; } = null!;
}
