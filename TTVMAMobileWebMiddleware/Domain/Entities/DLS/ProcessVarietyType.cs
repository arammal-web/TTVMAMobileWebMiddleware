using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a category or variety of a business process within the system.
/// </summary>
[Table("ProcessVarietyType", Schema = "BP")]
public partial class ProcessVarietyType
{
    /// <summary>
    /// Unique identifier for the Business Process Type
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Name of the Business Process Type in English
    /// </summary>
    /// <example>Production</example>
    [StringLength(50)]
    public string? NameEn { get; set; }

    /// <summary>
    /// Name of the Business Process Type in English
    /// </summary>
    /// <example>الانتاج</example>
    [StringLength(50)]
    public string? NameAr { get; set; }

    /// <summary>
    /// Name of the Business Process Type in English
    /// </summary>
    /// <example>Production</example>
    [StringLength(50)]
    public string? NameFr { get; set; }

    /// <summary>
    /// Identifier used during data migration
    /// </summary>
    /// <example>1</example>
    public int? MigrationId { get; set; }

    /// <summary>
    /// Indicates if the record is logically deleted
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Deletion timestamp if the record is deleted
    /// </summary>
    /// <example>2022-01-01 00:00:00.000</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who deleted the record
    /// </summary>
    /// <example>1</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date of record creation
    /// </summary>
    /// <example>2022-01-01 00:00:00.000</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record
    /// </summary>
    /// <example>0</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date of last modification
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
    /// Navigation property for the Status table
    /// </summary>
    [InverseProperty("ProcessVarietyType")]
    public virtual ICollection<ProcessVariety?> ProcessVarieties { get; set; } = new List<ProcessVariety>();
}
