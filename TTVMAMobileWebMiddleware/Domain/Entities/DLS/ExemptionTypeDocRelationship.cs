using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents the relationship between exemption types and required documents for specific processes.
/// </summary>
[Table("ExemptionTypeDocRelationship", Schema = "BP")]
public partial class ExemptionTypeDocRelationship
{
    /// <summary>
    /// Unique identifier for the exemption type-document relationship
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Reference to the business process
    /// </summary>
    /// <example>1001</example>
    public int? ProcessId { get; set; }

    /// <summary>
    /// Reference to the document
    /// </summary>
    /// <example>501</example>
    public int? DocumentId { get; set; }

    /// <summary>
    /// Reference to the exemption type
    /// </summary>
    /// <example>301</example>
    public int? ExemptionTypeId { get; set; }

    /// <summary>
    /// Indicates if the document is mandatory
    /// </summary>
    /// <example>true</example>
    public bool IsMandatory { get; set; }

    /// <summary>
    /// Indicates if the record is logically deleted
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date of record deletion (if applicable)
    /// </summary>
    /// <example>2025-01-30T15:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who marked the record as deleted
    /// </summary>
    /// <example>7</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date of record creation
    /// </summary>
    /// <example>2025-01-01T09:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record
    /// </summary>
    /// <example>3</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date of last modification
    /// </summary>
    /// <example>2025-01-15T14:45:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record
    /// </summary>
    /// <example>5</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Reference to the status
    /// </summary>
    /// <example>1</example>
    public int? StatusId { get; set; }

    /// <summary>
    /// Navigation property to the document linked with this exemption.
    /// </summary>
    [ForeignKey("DocumentId")]
    [InverseProperty("ExemptionTypeDocRelationships")]
    public virtual Document? Document { get; set; }

    /// <summary>
    /// Navigation property to the exemption type linked with this record.
    /// </summary>
    [ForeignKey("ExemptionTypeId")]
    [InverseProperty("ExemptionTypeDocRelationships")]
    public virtual ExemptionType? ExemptionType { get; set; }

    /// <summary>
    /// Navigation property to the business process.
    /// </summary>
    [ForeignKey("ProcessId")]
    [InverseProperty("ExemptionTypeDocRelationships")]
    public virtual Process? Process { get; set; }
}
