using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a category under which fees are grouped, with names and sorting logic.
/// </summary>
[Table("FeeCategory", Schema = "BP")]
public partial class FeeCategory
{
    /// <summary>
    /// Unique identifier for the fee category.
    /// </summary>
    /// <example>FEE-CAT-001</example>
    [Key] 
    public int Id { get; set; } 

    /// <summary>
    /// Fee category name in English.
    /// </summary>
    /// <example>Driving License Fees</example>
    [StringLength(30)]
    public string NameEn { get; set; } = null!;

    /// <summary>
    /// Fee category name in Arabic.
    /// </summary>
    /// <example>رسوم رخصة القيادة</example>
    [StringLength(30)]
    public string NameAr { get; set; } = null!;

    /// <summary>
    /// Fee category name in French.
    /// </summary>
    /// <example>Frais de permis de conduire</example>
    [StringLength(30)]
    public string? NameFr { get; set; }

    /// <summary>
    /// Sequence number for fee category sorting.
    /// </summary>
    /// <example>1</example>
    public int Sequence { get; set; }

    /// <summary>
    /// Primary expiry date for the fee category.
    /// </summary>
    /// <example>12/2025</example>
    [StringLength(20)]
    public string? ExpiryDate { get; set; }

    /// <summary>
    /// Additional expiry date for the fee category.
    /// </summary>
    /// <example>06/2026</example>
    [StringLength(20)]
    public string? AdditionalExpiryDate { get; set; }

    /// <summary>
    /// Reference to the Status table.
    /// </summary>
    /// <example>1</example>
    public int? StatusId { get; set; }

    /// <summary>
    /// Code representing the fee category.
    /// </summary>
    /// <example>DLC1001</example>
    [StringLength(50)]
    public string? Code { get; set; }

    /// <summary>
    /// ApplicationDomain of the fee category.
    /// </summary>
    /// <example>DriverLicensing</example>
    [StringLength(50)]
    public string? Domain { get; set; }

    /// <summary>
    /// Identifier used during migration.
    /// </summary>
    /// <example>1201</example>
    public int? MigrationID { get; set; }

    /// <summary>
    /// Logical deletion flag (1 = deleted, 0 = active).
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// ID of the user who deleted the fee category.
    /// </summary>
    /// <example>4</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date when the fee category was marked as deleted.
    /// </summary>
    /// <example>2025-01-31T14:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// Date the record was created.
    /// </summary>
    /// <example>2025-01-01T10:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// User who created the record.
    /// </summary>
    /// <example>2</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date the record was last modified.
    /// </summary>
    /// <example>2025-01-20T16:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// User who last modified the record.
    /// </summary>
    /// <example>5</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Navigation property for the fees under this fee category.
    /// </summary>
    [InverseProperty("FeeCategory")]
    public virtual ICollection<Fee?> Fees { get; set; } = new List<Fee>();

    public ICollection<ReceiptDetail> ReceiptDetails { get; set; } = new List<ReceiptDetail>();
}
