using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("Receipt")]
public partial class ReceiptMOB
{
    /// <summary>
    /// Unique identifier for each receipt record.
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Reference to the associated application for the receipt.
    /// </summary>
    /// <example>APP-2025-0001</example>
    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    /// <summary>
    /// Unique receipt number issued for the transaction.
    /// </summary>
    /// <example>RCPT-002345</example>
    [StringLength(50)]
    public string? ReceiptNumber { get; set; } = null!;

    /// <summary>
    /// Unique receipt number issued for the transaction.
    /// </summary>
    /// <example>002345</example> 
    [StringLength(50)]
    public string? ReceiptNumberIntegration { get; set; } = null!;
    /// <summary>
    /// Unique receipt number issued for the transaction.
    /// </summary>
    /// <example>RCPT-002345</example>
    [StringLength(50)]
    public string? ReceiptCategorySequenceNumber { get; set; } = null!;

    /// <summary>
    /// Optional description for the receipt.
    /// </summary>
    /// <example>Payment for license renewal</example>
    [StringLength(250)]
    public string? Description { get; set; }

    /// <summary>
    /// Status of the receipt (e.g., issued, paid).
    /// </summary>
    /// <example>1</example>
    public int ReceiptStatusId { get; set; }

    /// <summary>
    /// Date when the receipt status was last updated.
    /// </summary>
    /// <example>2025-01-15T10:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ReceiptStatusDate { get; set; }

    /// <summary>
    /// Reference to the structure associated with this receipt.
    /// </summary>
    /// <example>12</example>
    public int? StructureId { get; set; }

    /// <summary>
    /// Total amount specified on the receipt.
    /// </summary>
    /// <example>75.5</example>
    public float? TotalAmount { get; set; }

    /// <summary>
    /// Indicates if the receipt has been paid.
    /// </summary>
    /// <example>true</example>
    public bool IsPaid { get; set; }

    /// <summary>
    /// Date when the receipt was paid.
    /// </summary>
    /// <example>2025-01-16T14:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? PaidDate { get; set; }

    /// <summary>
    /// Reference number provided by the payment provider.
    /// </summary>
    /// <example>PMT-998877</example>
    [StringLength(50)]
    public string? PaymentProviderNumber { get; set; }

    /// <summary>
    /// Date of transaction confirmation from the payment provider.
    /// </summary>
    /// <example>2025-01-16T14:01:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? PaymentProviderDate { get; set; }

    /// <summary>
    /// Additional metadata or payload returned from the payment provider.
    /// </summary>
    /// <example>{"transactionId": "abc123"}</example>
    public string? PaymentProviderData { get; set; }

    /// <summary>
    /// Encrypted hash value used for receipt data integrity.
    /// </summary>
    /// <example>d1f546f...</example>
    [StringLength(1000)]
    public string? DataHash { get; set; }

    /// <summary>
    /// Indicates whether the receipt is posted to accounting.
    /// </summary>
    /// <example>true</example>
    public bool? IsPosted { get; set; }

    /// <summary>
    /// Date the receipt was posted.
    /// </summary>
    /// <example>2025-01-17T08:15:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? PostedDate { get; set; }

    /// <summary>
    /// User ID who posted the receipt.
    /// </summary>
    /// <example>21</example>
    public int? PostedUserId { get; set; }

    /// <summary>
    /// Full name of the citizen to whom the receipt was issued.
    /// </summary>
    /// <example>John Doe</example>
    [StringLength(1000)]
    public string? CitizenFullName { get; set; }

    /// <summary>
    /// Additional notes related to the receipt.
    /// </summary>
    /// <example>Paid via mobile app</example>
    [StringLength(2000)]
    public string? Notes { get; set; }

    /// <summary>
    /// Indicates whether the receipt is marked as deleted.
    /// </summary>
    /// <example>false</example>
    public bool? IsDeleted { get; set; }

    /// <summary>
    /// Date the receipt was marked as deleted.
    /// </summary>
    /// <example>2025-01-20T12:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who deleted the receipt.
    /// </summary>
    /// <example>8</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date the receipt record was created.
    /// </summary>
    /// <example>2025-01-10T08:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the receipt record.
    /// </summary>
    /// <example>2</example>
    public int? CreatedUserId { get; set; }

    /// <summary>
    /// Date the receipt record was last modified.
    /// </summary>
    /// <example>2025-01-15T10:30:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the receipt record.
    /// </summary>
    /// <example>4</example>
    public int? ModifiedUserId { get; set; }
    public int? DrivingLicenseId { get; set; }

    //[ForeignKey(nameof(DrivingLicenseId))]
    ///// <summary>
    ///// Navigation property for the associated DrivingLicense.
    ///// </summary>
    //public virtual DrivingLicenseABP? DrivingLicense { get; set; }

    /// <summary>
    /// Navigation property for the ReceiptDetails.
    /// ForeignKey of ReceiptId.
    /// Inverse Property for Receipt.
    /// </summary>
    //[ForeignKey("Id")]
    [InverseProperty("Receipt")]
    public virtual ICollection<ReceiptDetailMOB?> ReceiptDetails { get; set; } = new List<ReceiptDetailMOB>();

    public ApplicationMob? Application { get; set; }
    public Status? ReceiptStatus { get; set; } = null!;
}
