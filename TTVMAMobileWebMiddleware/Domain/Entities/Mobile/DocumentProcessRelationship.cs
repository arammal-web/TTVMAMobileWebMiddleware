using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[PrimaryKey("DocumentId", "ProcessId", "BPVarietyId")]
[Table("DocumentProcessRelationship")]
[Index("ProcessId", "BPVarietyId", "DocumentId", Name = "IX_DocumentProcessRelationship", IsUnique = true)]
public partial class DocumentProcessRelationship
{
    [Key]
    public int DocumentId { get; set; }

    [Key]
    public int ProcessId { get; set; }

    [Key]
    public int BPVarietyId { get; set; }

    public bool IsMandatory { get; set; }

    public int? MigrationId { get; set; }

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

    public int? StatusId { get; set; }

    public bool? IsRequired { get; set; }

    [ForeignKey("DocumentId")]
    [InverseProperty("DocumentProcessRelationships")]
    public virtual Document Document { get; set; } = null!;

    [ForeignKey("ProcessId, BPVarietyId")]
    [InverseProperty("DocumentProcessRelationships")]
    public virtual VarietyBusinessProcess VarietyBusinessProcess { get; set; } = null!;
}
