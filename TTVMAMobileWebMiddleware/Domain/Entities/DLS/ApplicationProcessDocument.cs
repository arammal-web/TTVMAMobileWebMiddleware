using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a document associated with a specific application and business process.
/// </summary>
[Table("ApplicationProcessDocument", Schema = "APP")]
public partial class ApplicationProcessDocument
{
    /// <summary>
    /// Unique identifier for the application process document.
    /// </summary>
    /// <example>101</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Reference to the application associated with the document.
    /// </summary>
    /// <example>APP-2025-0005</example>
    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    /// <summary>
    /// Identifier of the related business process.
    /// </summary>
    /// <example>3</example>
    public int ProcessId { get; set; }

    /// <summary>
    /// Identifier of the license type.
    /// </summary>
    /// <example>2</example>
    public int BPVarietyId { get; set; }

    /// <summary>
    /// Identifier of the document.
    /// </summary>
    /// <example>500</example>
    public int DocumentId { get; set; }

    /// <summary>
    /// Full path of the document file.
    /// </summary>
    /// <example>/docs/uploads/doc123.pdf</example>
    [StringLength(250)]
    public string? DocFilePath { get; set; }

    /// <summary>
    /// Title of the document.
    /// </summary>
    /// <example>Medical Certificate</example>
    [StringLength(250)]
    public string? DocTitle { get; set; }

    /// <summary>
    /// Keywords related to the document content.
    /// </summary>
    /// <example>Health, Certificate</example>
    [StringLength(500)]
    public string? DocKeyWord { get; set; }

    /// <summary>
    /// Binary content of the uploaded document file.
    /// </summary>
    public byte[]? DocFileData { get; set; }

    /// <summary>
    /// File extension of the uploaded document.
    /// </summary>
    /// <example>.pdf</example>
    [StringLength(10)]
    public string? DocFileExt { get; set; }

    /// <summary>
    /// Hash value of the uploaded file for verification.
    /// </summary>
    /// <example>a8c4f82304...</example>
    [StringLength(150)]
    public string? DocFileHash { get; set; }

    /// <summary>
    /// Additional notes about the document.
    /// </summary>
    /// <example>Original signed version attached.</example>
    [StringLength(250)]
    public string? Notes { get; set; }

    /// <summary>
    /// Status ID of the document.
    /// </summary>
    /// <example>1</example>
    public int? StatusId { get; set; }

    /// <summary>
    /// Indicates whether the record is marked as deleted.
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date when the record was deleted.
    /// </summary>
    /// <example>2025-05-01T14:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who deleted the record.
    /// </summary>
    /// <example>7</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date when the record was created.
    /// </summary>
    /// <example>2025-04-25T09:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record.
    /// </summary>
    /// <example>2</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date when the record was last modified.
    /// </summary>
    /// <example>2025-04-30T11:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record.
    /// </summary>
    /// <example>4</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Navigation reference to the related application process.
    /// </summary>
    [ForeignKey("ApplicationId, ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcessDocuments")]
    public virtual ApplicationProcess? ApplicationProcess { get; set; } = null!;

    /// <summary>
    /// Navigation reference to the document entity.
    /// </summary>
    [ForeignKey("DocumentId")]
    [InverseProperty("ApplicationProcessDocuments")]
    public virtual Document? Document { get; set; } = null!;

    /// <summary>
    /// Navigation reference to the document-process relationship.
    /// </summary>
    [ForeignKey("DocumentId, ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcessDocuments")]
    public virtual DocumentProcessRelationship? DocumentProcessRelationship { get; set; } = null!;
}
