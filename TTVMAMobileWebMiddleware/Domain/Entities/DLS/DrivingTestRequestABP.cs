 using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

[Table("DrivingTestRequest", Schema = "DLS")]
public partial class DrivingTestRequestABP
{
    /// <summary>
    /// Unique identifier for each driving test request record
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Reference to the application associated with this driving test request
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
    /// Status of the driving test request
    /// </summary>
    public int RequestStatusId { get; set; }

    /// <summary>
    /// Date when the driving test request status was last updated
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime RequestStatusDate { get; set; }

    /// <summary>
    /// Optional description of the driving test request
    /// </summary>
    [StringLength(50)]
    public string? Description { get; set; }

    /// <summary>
    /// Date when the driving test is scheduled
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime? TestDate { get; set; }

    /// <summary>
    /// Time when the driving test is scheduled
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime? TestTime { get; set; }

    /// <summary>
    /// Result of the driving test (if applicable)
    /// </summary>
    public int? TestExamResultId { get; set; }

    /// <summary>
    /// Reference to the structure where the driving test will take place
    /// </summary>
    public int StructureId { get; set; }

    /// <summary>
    /// Additional notes or comments about the driving test request
    /// </summary>
    [StringLength(2000)]
    public string? Notes { get; set; }

    /// <summary>
    /// Indicates whether the record has been marked as deleted
    /// </summary>
    public bool? IsDeleted { get; set; }

    /// <summary>
    /// Date and time when the record was marked as deleted
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// User identifier for who marked the record as deleted
    /// </summary>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date when the record was created
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime? CreatedDate { get; set; }

    /// <summary>
    /// User identifier for who created the record
    /// </summary>
    public int? CreatedUserId { get; set; }

    /// <summary>
    /// Date when the record was last modified
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// User identifier for who last modified the record
    /// </summary>
    public int? ModifiedUserId { get; set; }

    [StringLength(50)]
    public string ApplicationNumber { get; set; } = null!;

    [StringLength(50)]
    public string ReceiptNumber { get; set; } = null!;

    public int CitizenId { get; set; }

    public int? DrivingLicenseId { get; set; }

    public int? ExamListNumber { get; set; }

    public int? TestRequestLicenseTypeId { get; set; }

    public bool? IsManualAssignment { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? AssignmentDate { get; set; }

    [ForeignKey("RequestStatusId")]
    [InverseProperty("DrivingTestRequests")]
    public virtual RequestStatusABP RequestStatus { get; set; } = null!;

}
