using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a document file attached to a checklist item for a specific application process.
/// </summary>
[Table("ApplicationProcessCheckListDocFile", Schema = "APP")]
public partial class ApplicationProcessCheckListDocFile
{
    /// <summary>
    /// Unique identifier of the checklist document file.
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Reference to the application record.
    /// </summary>
    /// <example>APP-2025-001</example>
    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    /// <summary>
    /// Business process identifier.
    /// </summary>
    /// <example>3</example>
    public int ProcessId { get; set; }

    /// <summary>
    /// Reference to the associated BusinessProcesses table.
    /// </summary>
    /// <example>1</example>
    public int BPVarietyId { get; set; }

    /// <summary>
    /// Checklist item identifier.
    /// </summary>
    /// <example>15</example>
    public int ChekListId { get; set; }

    /// <summary>
    /// Path to the uploaded document file.
    /// </summary>
    /// <example>/path/to/file.pdf</example>
    [StringLength(250)]
    public string? DocFilePath { get; set; }

    /// <summary>
    /// Title of the document.
    /// </summary>
    /// <example>Document Title</example>
    [StringLength(250)]
    public string? DocTitle { get; set; }

    /// <summary>
    /// Keywords associated with the document.
    /// </summary>
    /// <example>keyword1, keyword2, keyword3</example>
    [StringLength(500)]
    public string? DocKeyWord { get; set; }

    /// <summary>
    /// Binary content of the document file.
    /// </summary>
    /// <example>NULL</example>
    [MaxLength(2000)]
    public byte[]? DocFileData { get; set; }

    /// <summary>
    /// File extension of the document (e.g. .pdf, .jpg).
    /// </summary>
    /// <example>pdf</example>
    [StringLength(10)]
    public string? DocFileExt { get; set; }

    /// <summary>
    /// Hash of the document for verification purposes.
    /// </summary>
    /// <example>NULL</example>
    [StringLength(150)]
    public string? DocFileHash { get; set; }

    /// <summary>
    /// Indicates whether the document is digitally signed.
    /// </summary>
    /// <example>0</example>
    public bool? IsSignedDocFile { get; set; }

    /// <summary>
    /// Public key associated with the signed document.
    /// </summary>
    /// <example>NULL</example>
    [StringLength(150)]
    public string? DocFilePublicKey { get; set; }

    /// <summary>
    /// Additional notes or comments about the document.
    /// </summary>
    /// <example>Notes</example>
    [StringLength(250)]
    public string? Notes { get; set; }

    /// <summary>
    /// Status identifier for the document (e.g. submitted, reviewed).
    /// </summary>
    /// <example>1</example>
    public int? StatusId { get; set; }

    /// <summary>
    /// Logical deletion flag.
    /// </summary>
    /// <example>0</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date when the document file was marked as deleted.
    /// </summary>
    /// <example>NULL</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who deleted the document.
    /// </summary>
    /// <example>NULL</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Record creation timestamp.
    /// </summary>
    /// <example>2023-06-01 12:34:56.789</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record.
    /// </summary>
    /// <example>1</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Record last modification timestamp.
    /// </summary>
    /// <example>NULL</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record.
    /// </summary>
    /// <example>NULL</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Navigation property to the related checklist item.
    /// </summary>
    [ForeignKey("ApplicationId, ProcessId, BPVarietyId, ChekListId")]
    [InverseProperty("ApplicationProcessCheckListDocFiles")]
    public virtual ApplicationProcessCheckList? ApplicationProcessCheckList { get; set; } = null!;
}