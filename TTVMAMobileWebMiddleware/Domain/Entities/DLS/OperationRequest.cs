using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

[Table("OperationRequest", Schema = "DLS")]
public partial class OperationRequestABP
{
    /// <summary>
    /// Unique identifier for each operation request record
    /// </summary>
    [Key]
    public int Id { get; set; } 

    /// <summary>
    /// Reference to the associated application for the operation request
    /// </summary>
    [StringLength(50)]
    public string? ApplicationId { get; set; } = null!;

    /// <summary>
    /// Reference to the business process related to the application
    /// </summary>
    public int ProcessId { get; set; }

    /// <summary>
    /// Reference to the associated BusinessProcesses table
    /// </summary>
    public int BPVarietyId { get; set; }

    /// <summary>
    /// Type of the operation being requested
    /// </summary>
    public int? OperationTypeId { get; set; }

    /// <summary>
    /// Status of the operation request
    /// </summary>
    public int? RequestStatusId { get; set; }

    /// <summary>
    /// Date when the operation request status was last updated
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime? RequestStatusDate { get; set; }

    /// <summary>
    /// Optional description or additional information for the operation request
    /// </summary>
    [StringLength(50)]
    public string? Description { get; set; }

    /// <summary>
    /// Reference to the structure associated with the operation request
    /// </summary>
    public int? StructureId { get; set; }

    /// <summary>
    /// Additional notes or comments regarding the operation request
    /// </summary>
    [StringLength(2000)]
    public string? Notes { get; set; }

    /// <summary>
    /// Operation Request Sequence Number (Application Squence Number)
    /// </summary> 
    public string? OperationRequestSequenceNumber { get; set; }

    /// <summary>
    /// Indicates whether the operation request is marked as deleted
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date when the operation request was marked as deleted
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// Identifier of the user who marked the operation request as deleted
    /// </summary>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date when the operation request was created
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Identifier of the user who created the operation request record
    /// </summary>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date when the operation request was last modified
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Identifier of the user who last modified the operation request record
    /// </summary>
    public int? ModifiedUserId { get; set; }

    [ForeignKey("OperationTypeId")]
    [InverseProperty("OperationRequests")]
    public virtual OperationTypeABP? OperationType { get; set; } = null!;

    [ForeignKey("RequestStatusId")]
    [InverseProperty("OperationRequests")]
    public virtual RequestStatusABP? RequestStatus { get; set; } = null!;

    [ForeignKey("StructureId")]
    [InverseProperty("OperationRequests")]
    public virtual StructureABP ? Structure { get; set; } = null!;

    [ForeignKey("ApplicationId")]
    [InverseProperty("OperationRequests")]
    public virtual ApplicationDLS? Application { get; set; } = null!;
} 