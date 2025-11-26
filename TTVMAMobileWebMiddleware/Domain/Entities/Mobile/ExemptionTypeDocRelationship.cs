using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("ExemptionTypeDocRelationship")]
public partial class ExemptionTypeDocRelationship
{
    [Key]
    public int Id { get; set; }

    public int? ProcessId { get; set; }

    public int? DocumentId { get; set; }

    public int? ExemptionTypeId { get; set; }

    public bool IsMandatory { get; set; }

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

    [ForeignKey("DocumentId")]
    [InverseProperty("ExemptionTypeDocRelationships")]
    public virtual Document? Document { get; set; }

    [ForeignKey("ExemptionTypeId")]
    [InverseProperty("ExemptionTypeDocRelationships")]
    public virtual ExemptionType? ExemptionType { get; set; }

    [ForeignKey("ProcessId")]
    [InverseProperty("ExemptionTypeDocRelationships")]
    public virtual Process? Process { get; set; }
}
