using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a document
/// </summary>
[Table("Documents" )]
public partial class Document
{
    /// <summary>
    /// Unique identifier for the document
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// English name of the document
    /// </summary>
    /// <example>Document 1</example>
    [StringLength(250)]
    public string DocumentNameEn { get; set; } = null!;

    /// <summary>
    /// Arabic name of the document
    /// </summary>
    /// <example>وثيقة 1</example>
    [StringLength(250)]
    public string DocumentNameAr { get; set; } = null!;

    /// <summary>
    /// French name of the document
    /// </summary>
    [StringLength(250)]
    public string? DocumentNameFr { get; set; }

    /// <summary>
    /// Code of the document
    /// </summary>
    /// <example>DOC1</example>
    [StringLength(50)]
    public string? DocumentCode { get; set; }

    /// <summary>
    /// Reference to the DocumentGroups table
    /// </summary>
    /// <example>1</example>
    public int? GroupId { get; set; }

    /// <summary>
    /// ApplicationDomain of the document
    /// </summary>
    /// <example>BP</example>
    [StringLength(50)]
    public string? Domain { get; set; }

    /// <summary>
    /// Reference to the Migration table
    /// </summary>
    /// <example>1</example>
    public int? MigrationId { get; set; }

    /// <summary>
    /// Logical deletion flag (1 = deleted, 0 = active)
    /// </summary>
    /// <example>0</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// ID of the user who deleted the record
    /// </summary>
    /// <example>1</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date when the record was deleted
    /// </summary>
    /// <example>2022-01-01 00:00:00.000</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// Date when the record was created
    /// </summary>
    /// <example>2022-01-01 00:00:00.000</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record
    /// </summary>
    /// <example>1</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date when the record was last modified
    /// </summary>
    /// <example>2022-01-01 00:00:00.000</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record
    /// </summary>
    /// <example>1</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Reference to the Status table
    /// </summary>
    /// <example>1</example>
    public int? StatusId { get; set; }

    /// <summary>
    ///  Binding Fields for each document 
    /// </summary>
    /// <example>1</example>
    public string? BindingColumn { get; set; } 

    /// <summary>
    /// Collection of ApplicationProcessDocuments
    /// </summary>
    [InverseProperty("Document")]
    public virtual ICollection<ApplicationProcessDocument?> ApplicationProcessDocuments { get; set; } = new List<ApplicationProcessDocument>();

    /// <summary>
    /// Collection of DocumentProcessRelationship
    /// </summary>
    [InverseProperty("Document")]
    public virtual ICollection<DocumentProcessRelationship?> DocumentProcessRelationships { get; set; } = new List<DocumentProcessRelationship>();

    /// <summary>
    /// Collection of ExemptionTypeDocRelationship
    /// </summary>
    [InverseProperty("Document")]
    public virtual ICollection<ExemptionTypeDocRelationship?> ExemptionTypeDocRelationships { get; set; } = new List<ExemptionTypeDocRelationship>();

    /// <summary>
    /// Reference to the DocumentGroups table
    /// </summary>
    [ForeignKey("GroupId")]
    [InverseProperty("Documents")]
    public virtual DocumentGroup? Group { get; set; }
}
