using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("Domain")]
public partial class DomainMob
{
    [Key]
    [StringLength(3)]
    [Unicode(false)]
    public string Id { get; set; } = null!;

    [StringLength(50)]
    public string DescriptionEn { get; set; } = null!;

    [StringLength(50)]
    public string DescriptionAr { get; set; } = null!;

    [StringLength(50)]
    public string DescriptionFr { get; set; } = null!;

    [StringLength(500)]
    public string? Note { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }
     
}
