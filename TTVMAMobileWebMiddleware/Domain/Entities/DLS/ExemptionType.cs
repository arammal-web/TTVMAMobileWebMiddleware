using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents an exemption type for a business process.
/// </summary>
[Table("ExemptionType", Schema = "BP")]
public partial class ExemptionType
{
    /// <summary>
    /// Unique identifier for the exemption type
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Description of the exemption type in English
    /// </summary>
    /// <example>Exemption Type 1</example>
    [StringLength(200)]
    public string? ExemptionTypeDescEn { get; set; }

    /// <summary>
    /// Description of the exemption type in Arabic
    /// </summary>
    /// <example>Exemption Type 1</example>
    [StringLength(200)]
    public string? ExemptionTypeDescAr { get; set; }

    /// <summary>
    /// Description of the exemption type in French
    /// </summary>
    /// <example>Exemption Type 1</example>
    [StringLength(200)]
    public string? ExemptionTypeDescFr { get; set; }
    /// <summary>
    /// Reference to the ownership type
    /// </summary>
    /// <example>1</example>
    public int? OwnershipTypeId { get; set; }

    /// <summary>
    /// Total number of exemptions allowed
    /// </summary>
    /// <example>1</example>
    public int? TotalNumberOfExemption { get; set; }

    /// <summary>
    /// Logical deletion flag (1 = deleted, 0 = active)
    /// </summary>
    /// <example>0</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date when the record was created
    /// </summary>
    /// <example>2023-01-01T00:00:00.000Z</example>
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
    /// <example>2023-01-01T00:00:00.000Z</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record
    /// </summary>
    /// <example>1</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Reference to the status table
    /// </summary>
    /// <example>1</example>
    public int? StatusId { get; set; }

    /// <summary>
    /// Unique code for the exemption type
    /// </summary>
    /// <example>EX1</example>
    [StringLength(50)]
    public string? Code { get; set; }

    /// <summary>
    /// Collection of documents associated with the exemption type
    /// </summary>
    [InverseProperty("ExemptionType")]
    public virtual ICollection<ExemptionTypeDocRelationship?> ExemptionTypeDocRelationships { get; set; } = new List<ExemptionTypeDocRelationship>();

    /// <summary>
    /// Collection of fees associated with the exemption type
    /// </summary>
    [InverseProperty("ExemptionType")]
    public virtual ICollection<ProcessExemptionFee?> ProcessExemptionFees { get; set; } = new List<ProcessExemptionFee>();
}
