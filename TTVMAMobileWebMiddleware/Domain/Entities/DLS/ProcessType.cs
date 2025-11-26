using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Business Process Type
/// </summary>
[Table("ProcessType", Schema = "BP")]
public partial class ProcessType
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
    /// <example>Driving License Issuance</example>
    [StringLength(50)]
    public string? NameEn { get; set; }

    /// <summary>
    /// Name of the Business Process Type in English
    /// </summary>
    /// <example>تصدير رخصة القيادة</example>
    [StringLength(50)]
    public string? NameAr { get; set; }

    /// <summary>
    /// Name of the Business Process Type in English
    /// </summary>
    /// <example>Driving License Issuance</example>
    [StringLength(50)]
    public string? NameFr { get; set; }

    /// <summary>
    /// Reference to the type of the Business Process Type
    /// </summary>
    /// <example>1</example>
    public int? MigrationId { get; set; }

    /// <summary>
    /// Reference to the type of the Business Process Type
    /// </summary>
    /// <example>Driving License Issuance</example>
    [StringLength(50)]
    public string? MigrationDLSFeeType { get; set; }

    /// <summary>
    /// Indicates if the record is logically deleted
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date of record deletion
    /// </summary>
    /// <example>NULL</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who deleted the record
    /// </summary>
    /// <example>NULL</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date of record creation
    /// </summary>
    /// <example>2025-01-15T00:00:00</example>
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
    /// <example>NULL</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record
    /// </summary>
    /// <example>NULL</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Reference to the Status table
    /// </summary>
    /// <example>1</example>
    public int? StatusId { get; set; }

    /// <summary>
    /// Navigation property to the Process table
    /// </summary>
    [InverseProperty("Type")]
    public virtual ICollection<Process?> Processes { get; set; } = new List<Process>();
}
