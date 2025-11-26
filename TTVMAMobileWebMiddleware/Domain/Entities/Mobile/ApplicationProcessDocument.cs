using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("ApplicationProcessDocument")]
public partial class ApplicationProcessDocument
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    public int ProcessId { get; set; }

    public int BPVarietyId { get; set; }

    public int DocumentId { get; set; }

    [StringLength(250)]
    public string? DocFilePath { get; set; }

    [StringLength(250)]
    public string? DocTitle { get; set; }

    [StringLength(500)]
    public string? DocKeyWord { get; set; }

    public byte[]? DocFileData { get; set; }

    [StringLength(10)]
    public string? DocFileExt { get; set; }

    [StringLength(150)]
    public string? DocFileHash { get; set; }

    [StringLength(250)]
    public string? Notes { get; set; }

    public int? StatusId { get; set; }
 
    public bool IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    public int? DeletedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    [ForeignKey("ApplicationId, ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcessDocuments")]
    [JsonIgnore]
    public virtual ApplicationProcess ApplicationProcess { get; set; } = null!;

    [ForeignKey("DocumentId")]
    [InverseProperty("ApplicationProcessDocuments")]
    [JsonIgnore]
    public virtual Document Document { get; set; } = null!;

    /// <summary>
    /// Navigation to document reviews
    /// </summary>
    [InverseProperty("Document")]
    [JsonIgnore]
    public virtual ICollection<DocumentReview> DocumentReviews { get; set; } = new List<DocumentReview>();
}
