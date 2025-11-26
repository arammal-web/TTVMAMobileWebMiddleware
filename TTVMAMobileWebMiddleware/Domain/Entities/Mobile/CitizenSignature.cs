using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("CitizenSignature")]
public partial class CitizenSignature
{
    [Key]
    public int Id { get; set; }

    public int CitizenId { get; set; }

    public byte[]? Signature { get; set; }

    [StringLength(64)]
    public string? SignatureHash { get; set; }

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

    [ForeignKey("CitizenId")]
    [InverseProperty("CitizenSignatures")]
    public virtual Citizen? Citizen { get; set; } = null!;
}
