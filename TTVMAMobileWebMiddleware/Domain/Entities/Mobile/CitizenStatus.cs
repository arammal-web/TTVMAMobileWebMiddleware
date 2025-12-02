using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("CitizenStatus")]
public partial class CitizenStatus
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string? NameEn { get; set; }

    [StringLength(50)]
    public string? NameAr { get; set; }

    [StringLength(50)]
    public string? NameFr { get; set; }
}

