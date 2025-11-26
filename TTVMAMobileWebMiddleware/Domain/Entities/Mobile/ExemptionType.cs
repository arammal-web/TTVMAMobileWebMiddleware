using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("ExemptionType")]
public partial class ExemptionType
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string? ExemptionTypeDescEn { get; set; }

    [StringLength(200)]
    public string? ExemptionTypeDescAr { get; set; }

    public int? OwnershipTypeId { get; set; }

    public int? TotalNumberOfExemption { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    public int? StatusId { get; set; }

    [StringLength(50)]
    public string? Code { get; set; }

    [StringLength(50)]
    public string? ExemptionTypeDescFr { get; set; }

    [InverseProperty("ExemptionType")]
    public virtual ICollection<ExemptionTypeDocRelationship> ExemptionTypeDocRelationships { get; set; } = new List<ExemptionTypeDocRelationship>();

    [InverseProperty("ExemptionType")]
    public virtual ICollection<ProcessExemptionFee> ProcessExemptionFees { get; set; } = new List<ProcessExemptionFee>();

    [ForeignKey("StatusId")]
    [InverseProperty("ExemptionTypes")]
    public virtual Status? Status { get; set; }
}
