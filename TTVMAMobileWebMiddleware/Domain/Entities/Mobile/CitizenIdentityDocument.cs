using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("CitizenIdentityDocument")]
public partial class CitizenIdentityDocument
{
    [Key]
    public int Id { get; set; }

    public int CitizenId { get; set; }

    public int DocumentId { get; set; }

    public int ProcessId { get; set; }

    public byte[]? IdentityDocument { get; set; }

    [StringLength(64)]
    public string? IdentityDocumentHash { get; set; }

    public string? IdentityDocumentData { get; set; }

    [StringLength(250)]
    public string? Description { get; set; }

    public int? StructureId { get; set; }

    [StringLength(2000)]
    public string? Notes { get; set; }

    public bool? IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    public int? DeletedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? CreatedDate { get; set; }

    public int? CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    [StringLength(10)]
    public string? DocFileExt { get; set; }

    [ForeignKey("CitizenId")]
    [InverseProperty("CitizenIdentityDocuments")]
    public virtual Citizen? Citizen { get; set; } = null!;

    [ForeignKey("DocumentId")]
    [InverseProperty("CitizenIdentityDocuments")]
    public virtual Document? Document { get; set; } = null!;
}
