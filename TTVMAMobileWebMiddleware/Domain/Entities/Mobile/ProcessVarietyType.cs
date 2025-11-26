using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("ProcessVarietyType")]
public partial class ProcessVarietyType
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string? NameEn { get; set; }

    [StringLength(50)]
    public string? NameAr { get; set; }

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

    [StringLength(50)]
    public string? NameFr { get; set; }

    [InverseProperty("ProcessVarietyType")]
    public virtual ICollection<ProcessVariety> ProcessVarieties { get; set; } = new List<ProcessVariety>();
}
