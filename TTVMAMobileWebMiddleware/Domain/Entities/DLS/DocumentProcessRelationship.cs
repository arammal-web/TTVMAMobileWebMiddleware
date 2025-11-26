using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents the relationship between documents and specific business processes with their license type (variety).
/// </summary>
[PrimaryKey("DocumentId", "ProcessId", "BPVarietyId")]
[Table("DocumentProcessRelationship", Schema = "BP")]
[Index("ProcessId", "BPVarietyId", "DocumentId", Name = "IX_DocumentProcessRelationship", IsUnique = true)]
public partial class DocumentProcessRelationship
{
    /// <summary>
    /// Reference to the document
    /// </summary>
    /// <example>401</example>
    [Key]
    public int DocumentId { get; set; }

    /// <summary>
    /// Reference to the business process
    /// </summary>
    /// <example>1001</example>
    [Key]
    public int ProcessId { get; set; }

    /// <summary>
    /// Reference to the license type
    /// </summary>
    /// <example>2002</example>
    [Key]
    public int BPVarietyId { get; set; }

    /// <summary>
    /// Indicates if the relationship is mandatory
    /// </summary>
    /// <example>true</example>
    public bool IsMandatory { get; set; }

    /// <summary>
    /// Indicates if the relationship is required
    /// </summary>
    public  bool IsRequired { get; set; }

    /// <summary>
    /// Identifier used during data migration
    /// </summary>
    /// <example>12</example>
    public int? MigrationId { get; set; }

    /// <summary>
    /// Indicates if the record is logically deleted
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date the record was marked as deleted
    /// </summary>
    /// <example>2025-02-01T14:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who marked the record as deleted
    /// </summary>
    /// <example>4</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date of record creation
    /// </summary>
    /// <example>2025-01-01T10:00:00</example>
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
    /// <example>2025-01-15T16:30:00</example>
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
    /// Navigation property for the documents uploaded under this document-process relationship.
    /// </summary>
    [InverseProperty("DocumentProcessRelationship")]
    public virtual ICollection<ApplicationProcessDocument?> ApplicationProcessDocuments { get; set; } = new List<ApplicationProcessDocument>();

    /// <summary>
    /// Navigation reference to the document entity.
    /// </summary>
    [ForeignKey("DocumentId")]
    [InverseProperty("DocumentProcessRelationships")]
    public virtual Document? Document { get; set; } = null!;

    /// <summary>
    /// Navigation reference to the variety-specific business process.
    /// </summary>
    [ForeignKey("ProcessId, BPVarietyId")]
    [InverseProperty("DocumentProcessRelationships")]
    public virtual VarietyBusinessProcess? VarietyBusinessProcess { get; set; } = null!;
}
