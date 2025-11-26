using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("ProcessType")]
public partial class ProcessType
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string? NameEn { get; set; }

    [StringLength(50)]
    public string? NameAr { get; set; }

    public int? MigrationId { get; set; }

    [StringLength(50)]
    public string? MigrationDLSFeeType { get; set; }

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

    [InverseProperty("Type")]
    public virtual ICollection<Process> Processes { get; set; } = new List<Process>();
}
