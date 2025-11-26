using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents the details of each item charged in a receipt.
/// </summary>
[Table("ReceiptDetail", Schema = "APP")]
public partial class ReceiptDetail
{
    /// <summary>
    /// Unique identifier for each receipt detail record.
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Reference to the associated receipt.
    /// </summary>
    /// <example>1001</example>
    public int ReceiptId { get; set; }

    /// <summary>
    /// Reference to the item being billed in the receipt.
    /// </summary>
    /// <example>501</example>
    public int ItemId { get; set; }
    /// <summary>
    /// Identifier for the process the receipt is associated with.
    /// </summary>
    /// <example>101</example>
    public int ProcessId { get; set; }

    /// <summary>
    /// Identifier for the process variety linked to the receipt.
    /// </summary>
    /// <example>205</example>
    public int BPVarietyId { get; set; }
    /// <summary>
    /// Arabic description of the item being billed.
    /// </summary>
    /// <example>رسم إصدار الرخصة</example>
    [StringLength(250)]
    public string ItemDescriptionAR { get; set; } = null!;

    /// <summary>
    /// English description of the item being billed.
    /// </summary>
    /// <example>License issuance fee</example>
    [StringLength(250)]
    public string ItemDescriptionEN { get; set; } = null!;

    /// <summary>
    /// Code of the item being billed.
    /// </summary>
    /// <example>LICFEE001</example>
    [StringLength(50)]
    public string ItemCode { get; set; } = null!;

    /// <summary>
    /// Reference to the item type (if applicable).
    /// </summary>
    /// <example>2</example>
    public int? ItemTypeId { get; set; }

    /// <summary>
    /// Reference to the item category being billed.
    /// </summary>
    /// <example>3</example>
    public int? ItemCategoryId { get; set; }

    /// <summary>
    /// Amount charged for the item in the receipt.
    /// </summary>
    /// <example>150.75</example>
    public float Amount { get; set; }

    /// <summary>
    /// Additional notes or comments for the receipt detail.
    /// </summary>
    /// <example>Urgent processing fee</example>
    [StringLength(2000)]
    public string? Notes { get; set; }

    /// <summary>
    /// Indicates whether the receipt detail is marked as deleted.
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date when the receipt detail was marked as deleted.
    /// </summary>
    /// <example>2025-01-22T13:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// Identifier of the user who marked the receipt detail as deleted.
    /// </summary>
    /// <example>8</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date when the receipt detail record was created.
    /// </summary>
    /// <example>2025-01-10T08:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Identifier of the user who created the receipt detail record.
    /// </summary>
    /// <example>2</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date when the receipt detail record was last modified.
    /// </summary>
    /// <example>2025-01-20T10:45:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Identifier of the user who last modified the receipt detail record.
    /// </summary>
    /// <example>4</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Navigation property for the Receipt.
    /// ForeignKey of ReceiptId.
    /// Inverse Property for Receipt Details.
    /// </summary>
    [ForeignKey("ReceiptId")]
    [InverseProperty("ReceiptDetails")]
    public virtual Receipt? Receipt { get; set; } = null!;


    [ForeignKey("ItemCategoryId")]
    [InverseProperty("ReceiptDetails")]
    public virtual FeeCategory? FeeCategory { get; set; }
     

}
