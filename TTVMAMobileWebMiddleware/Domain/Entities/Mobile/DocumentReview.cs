using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TTVMAMobileWebMiddleware.Domain.Enums;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

/// <summary>
/// Represents a review of a document uploaded by an online citizen
/// </summary>
[Table("DocumentReview")]
public partial class DocumentReview
{
    /// <summary>
    /// Primary key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Foreign key to ApplicationProcessDocument (online document)
    /// </summary>
    public int DocId { get; set; }

    /// <summary>
    /// Review status (Valid, Invalid, Missing)
    /// </summary>
    public DocumentReviewStatus Status { get; set; }

    /// <summary>
    /// User ID who reviewed the document
    /// </summary>
    public int ReviewerUserId { get; set; }

    /// <summary>
    /// UTC timestamp when the review was performed
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime ReviewedAtUtc { get; set; }

    /// <summary>
    /// Optional notes about the review
    /// </summary>
    [StringLength(1000)]
    public string? Notes { get; set; }

    /// <summary>
    /// Navigation to the document
    /// </summary>
    [ForeignKey("DocId")]
    [InverseProperty("DocumentReviews")]
    public virtual ApplicationProcessDocument? Document { get; set; }
}

