using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a group of documents associated with a business process.
/// </summary>
[Table("DocumentGroups", Schema = "BP")]
public partial class DocumentGroup
{
    /// <summary>
    /// Unique identifier for the document group
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Code for the document group
    /// </summary>
    /// <example>1</example>
    public int? GroupCode { get; set; }

    /// <summary>
    /// English name of the document group
    /// </summary>
    /// <example>Documents</example>
    [StringLength(100)]
    public string GroupNameEn { get; set; } = null!;

    /// <summary>
    /// Arabic name of the document group
    /// </summary>
    /// <example>الوثائق</example>
    [StringLength(100)]
    public string? GroupNameAr { get; set; }

    /// <summary>
    /// French name of the document group
    /// </summary>
    [StringLength(100)]
    public string? GroupNameFr { get; set; }
    /// <summary>
    /// Selection flag indicating choice (e.g., Yes/No)
    /// </summary>
    /// <example>Y</example>
    [StringLength(1)]
    public string? Choose { get; set; }

    /// <summary>
    /// ApplicationDomain classification (e.g., licensing, registration)
    /// </summary>
    /// <example>Registration</example>
    [StringLength(50)]
    public string? Domain { get; set; }

    /// <summary>
    /// ID of the migration record
    /// </summary>
    /// <example>1</example>
    public int? MigrationId { get; set; }

    /// <summary>
    /// Logical deletion flag (1 = deleted, 0 = active)
    /// </summary>
    /// <example>0</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date when the record was created
    /// </summary>
    /// <example>2022-01-01</example>
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
    /// <example>2022-01-01</example>
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
    /// Collection of documents associated with the document group
    /// </summary>
    [InverseProperty("Group")]
    public virtual ICollection<Document?> Documents { get; set; } = new List<Document>();
}
