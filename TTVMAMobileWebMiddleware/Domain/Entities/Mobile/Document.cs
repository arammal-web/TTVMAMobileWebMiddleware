using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

public partial class Document
{
    [Key]
    public int Id { get; set; }

    [StringLength(250)]
    public string DocumentNameEn { get; set; } = null!;

    [StringLength(250)]
    public string DocumentNameAr { get; set; } = null!;

    [StringLength(50)]
    public string? DocumentCode { get; set; }

    public int? GroupId { get; set; }

    [StringLength(50)]
    public string? Domain { get; set; }

    public int? MigrationId { get; set; }

    public bool IsDeleted { get; set; }

    public int? DeletedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    public int? StatusId { get; set; }

    [StringLength(250)]
    public string? DocumentNameFr { get; set; }

    [StringLength(50)]
    public string? Icon { get; set; }

    [InverseProperty("Document")]
    public virtual ICollection<ApplicationProcessDocument> ApplicationProcessDocuments { get; set; } = new List<ApplicationProcessDocument>();

    [InverseProperty("Document")]
    public virtual ICollection<CitizenIdentityDocument> CitizenIdentityDocuments { get; set; } = new List<CitizenIdentityDocument>();

    [InverseProperty("Document")]
    public virtual ICollection<DocumentProcessRelationship> DocumentProcessRelationships { get; set; } = new List<DocumentProcessRelationship>();

    [InverseProperty("Document")]
    public virtual ICollection<ExemptionTypeDocRelationship> ExemptionTypeDocRelationships { get; set; } = new List<ExemptionTypeDocRelationship>();

    [ForeignKey("GroupId")]
    [InverseProperty("Documents")]
    public virtual DocumentGroup? Group { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("Documents")]
    public virtual Status? Status { get; set; }
}
